using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using LanguageExt;
using Serilog;

namespace ItHappened.Application
{
    public class CustomizationService : ICustomizationService
    {
        public CustomizationService(IRepository<Tracker> trackerRepository, IRepository<Event> eventRepository,
            IRepository<Comment> commentRepository, IRepository<Rating> ratingRepository,
            IRepository<Scale> scaleRepository, IRepository<Geotag> geotagRepository, 
            IRepository<Photo> photoRepository)
        {
            _trackerRepository = trackerRepository;
            _eventRepository = eventRepository;
            
            _commentRepository = commentRepository;
            _ratingRepository = ratingRepository;
            _scaleRepository = scaleRepository;
            _geotagRepository = geotagRepository;
            _photoRepository = photoRepository;
        }

        public void AddCommentToEvent(Guid actorId, Guid eventId, CommentForm form)
        {
            if (!form.IsNull() && form.IsCorrectlyFilled())
            {
                var comment = new Comment(Guid.NewGuid(), eventId, form.Content);
                AddEventCustomizationData(actorId, eventId, comment, _commentRepository);
            }
        }
        
        public void AddRatingToEvent(Guid actorId, Guid eventId, RatingForm form)
        {
            if (!form.IsNull() && form.IsCorrectlyFilled())
            {
                var rating = new Rating(Guid.NewGuid(), eventId, (Rating.StarsRating)form.Stars);
                AddEventCustomizationData(actorId, eventId, rating, _ratingRepository);
            }
        }

        public void AddScaleToEvent(Guid actorId, Guid eventId, ScaleForm form)
        {
            if (!form.IsNull() && form.IsCorrectlyFilled())
            {
                var scale = new Scale(Guid.NewGuid(), eventId, form.Scale);
                AddEventCustomizationData(actorId, eventId, scale, _scaleRepository);
            }
        }

        public void AddGeotagToEvent(Guid actorId, Guid eventId, GeotagForm form)
        {
            if (!form.IsNull() && form.IsCorrectlyFilled())
            {
                var geotag = new Geotag(Guid.NewGuid(), eventId, form.Longitude, form.Latitude);
                AddEventCustomizationData(actorId, eventId, geotag, _geotagRepository);
            }
        }

        public Option<Comment> GetComment(Guid actorId, Guid eventId)
        {
            return GetEventCustomizationData<Comment>(actorId, eventId, _commentRepository);
        }

        public Option<Rating> GetRating(Guid actorId, Guid eventId)
        {
            return GetEventCustomizationData<Rating>(actorId, eventId, _ratingRepository);
        }

        public Option<Scale> GetScale(Guid actorId, Guid eventId)
        {
            return GetEventCustomizationData<Scale>(actorId, eventId, _scaleRepository);
        }

        public Option<Geotag> GetGeotag(Guid actorId, Guid eventId)
        {
            return GetEventCustomizationData<Geotag>(actorId, eventId, _geotagRepository);
        }

        public bool CheckTrackerHasCustomizationOfSuchDataType(IEnumerable<CustomizationType> customizations,
            IEventCustomizationData data)
        {
            if (!customizations.IsNull())
            {
                foreach (var customization in customizations)
                {
                    var dataString = data.ToString().Split('.');
                    if (customization.ToString() == dataString[dataString.Length - 1])
                        return true;
                }
            }
            return false;
        }
        
        private Option<Tracker> CheckActorCanAddOrGetEvent(Guid actorId, Guid eventId)
        {
            var optionEvent = _eventRepository.Get(eventId);
            return optionEvent.Match(
                Some: @event =>
                {
                    var trackers = _trackerRepository.GetAll();
                    foreach (var tracker in trackers)
                    {
                        if (tracker.Id == @event.TrackerId)
                        {
                            if(tracker.UserId == actorId)
                                return tracker;
                            Log.Information($"User {actorId} tried to get or add events to tracker " +
                                            $"of user {tracker.UserId}");
                        }
                    }
                    return Option<Tracker>.None;
                },
                None: Option<Tracker>.None);
        }
        
        private void AddEventCustomizationData<T>(Guid actorId, Guid eventId, T data, IRepository<T> repository) 
            where T : IEventCustomizationData
        {
            var optionTracker = CheckActorCanAddOrGetEvent(actorId, eventId);
            optionTracker.Do(tracker =>
            {
                var splittedStringType = data.ToString().Split('.');
                var customizationType = splittedStringType[splittedStringType.Length - 1];
                if (CheckTrackerHasCustomizationOfSuchDataType(tracker.Customizations, data))
                {
                    repository.Save(data);
                    Log.Information($"Customization {customizationType} added to {eventId}");
                }
                else
                    Log.Error($"Tracker {tracker.Id} doesn't have customization" +
                              $"{customizationType} to add it to {eventId}");
            });
        }

        private Option<T> GetEventCustomizationData<T>(Guid actorId, Guid eventId, IRepository<T> repository)
        where T : IEventCustomizationData
        {
            if (!CheckActorCanAddOrGetEvent(actorId, eventId).IsSome)
                return Option<T>.None;

            var eventDatas = repository.GetAll();
            foreach (var eventData in eventDatas)
            {
                if (eventData.EventId == eventId)
                    return eventData;
            }
            return Option<T>.None;
        }

        private readonly IRepository<Tracker> _trackerRepository;
        private readonly IRepository<Event> _eventRepository;
        
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Rating> _ratingRepository;
        private readonly IRepository<Scale> _scaleRepository;
        private readonly IRepository<Geotag> _geotagRepository;
        private readonly IRepository<Photo> _photoRepository;
    }
}