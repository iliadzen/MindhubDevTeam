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
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
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
        
        private Option<Tracker> GetTrackerByEventId(Guid eventId)
        {
            var optionEvent = _eventRepository.Get(eventId);
            return optionEvent.Match(
                Some: @event =>
                {
                    var trackers = _trackerRepository.GetAll();
                    foreach (var tracker in trackers)
                    {
                        if (tracker.Id == @event.TrackerId)
                            return tracker;
                    }
                    return Option<Tracker>.None;
                },
                None: Option<Tracker>.None);
        }
        
        private void AddEventCustomizationData<T>(Guid actorId, Guid eventId, T data, IRepository<T> repository) 
            where T : IEventCustomizationData
        {
            var optionTracker = GetTrackerByEventId(eventId);
            optionTracker.Do(tracker =>
            {
                if (actorId == tracker.UserId)
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
                }
                else
                    Log.Information($"User {actorId} tried to customize event of user {tracker.UserId}");
            });
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