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
            IRepository<Comment> commentRepository)
        {
            _trackerRepository = trackerRepository;
            _eventRepository = eventRepository;
            _commentRepository = commentRepository;
        }

        public void AddCommentToEvent(Guid actorId, Guid eventId, CommentForm form)
        {
            if (!form.IsNull())
            {
                if (!string.IsNullOrEmpty(form.Content))
                {
                    var comment = new Comment(Guid.NewGuid(), eventId, form.Content);
                    AddEventCustomizationData(actorId, eventId, comment, _commentRepository);
                }
                Log.Error("Content of comment is null or empty");
            }
            else
                Log.Error("Comment form is null");
        }

        public void AddTrackerCustomization(Guid actorId, Guid trackerId, CustomizationType type)
        {
            var optionTracker = _trackerRepository.Get(trackerId);
            optionTracker.Do(tracker =>
            {
                if (actorId == tracker.UserId)
                {
                    if (!tracker.Customizations.Contains(type))
                    {
                        tracker.Customizations.Add(type);
                        _trackerRepository.Update(tracker);
                        Log.Information($"Customization {type.ToString()} added to tracker {trackerId}");
                    }
                    else
                        Log.Error(
                            $"Tracker {trackerId} is already have customization {type.ToString()}");
                }
                else
                    Log.Information(
                        $"User {actorId} tried to customize tracker of user {tracker.UserId}");
            });
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
                        if (!CheckEventCustomizationDataOfSuchTypeAddedToEvent(eventId, _commentRepository))
                        {
                            repository.Save(data);
                            Log.Information($"Customization {customizationType} " +
                                            $"added to {eventId}");
                        }
                        else
                            Log.Error($"Event {eventId} already has customization of " +
                                      $"{customizationType} type");
                    }
                    else
                    {
                        Log.Error($"Tracker {tracker.Id} doesn't have customization" +
                                  $"{customizationType} to add it to {eventId}");
                    }
                }
                else
                    Log.Information(
                        $"User {actorId} tried to customize event of user {tracker.UserId}");
            });
        }

        public bool CheckEventCustomizationDataOfSuchTypeAddedToEvent<T>(Guid eventId, IRepository<T> repository)
        where T : IEventCustomizationData
        {
            var eventDatas = repository.GetAll();
            foreach (var eventData in eventDatas)
            {
                if (eventData.EventId == eventId)
                    return true;
            }
            return false;
        }
        
        private readonly IRepository<Tracker> _trackerRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Comment> _commentRepository;
    }
}