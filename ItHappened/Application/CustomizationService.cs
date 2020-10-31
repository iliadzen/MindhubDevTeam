using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;
using Serilog;

namespace ItHappened.Application
{
    public class CustomizationService : ICustomizationService
    {
        public CustomizationService(IRepository<Tracker> trackerRepository, IRepository<Event> eventRepository,
            IRepository<EventCustomization> eventCustomizationRepository)
        {
            _trackerRepository = trackerRepository;
            _eventRepository = eventRepository;
            _eventCustomizationRepository = eventCustomizationRepository;
        }
        
        public void AddEventCustomizationData(Guid actorId, Guid eventId, EventCustomizationData data)
        {
            var optionCustomization = GetEventCustomizationByEventId(actorId, eventId);
            optionCustomization.Do(eventData =>
            {
                var optionTracker = GetTrackerByEventId(eventId);
                optionTracker.Do(tracker =>
                {
                    if (CheckTrackerHasCustomizationOfSuchDataType(tracker.Customizations, data))
                    {
                        eventData.AddCustomization(data);
                        Log.Information($"Customization {data.ToString()} added to {eventId}");
                    }
                    else
                        Log.Error($"Tracker {tracker.Id} doesn't have customization" +
                                  $"{data.ToString()} to add it to {eventId}");
                });
            });
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

        public Option<EventCustomization> GetEventCustomizationByEventId(Guid actorId, Guid eventId)
        {
            var optionEvent = _eventRepository.Get(eventId);
            return optionEvent.Match(
                Some: @event =>
                {
                    if (actorId == @event.Id)
                    {
                        var eventDatas = _eventCustomizationRepository.GetAll();
                        foreach (var eventData in eventDatas)
                        {
                            if (eventData.EventId == eventId)
                                return eventData;
                        }
                        return Option<EventCustomization>.None;
                    }
                    return Option<EventCustomization>.None;
                },
                None: Option<EventCustomization>.None);
        }

        public Option<Tracker> GetTrackerByEventId(Guid eventId)
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

        public bool CheckTrackerHasCustomizationOfSuchDataType(ISet<CustomizationType> customizations,
            EventCustomizationData data)
        {
            foreach (var customization in customizations)
            {
                var dataString = data.ToString().Split('.');
                if (customization.ToString() == dataString[dataString.Length - 1])
                    return true;
            }
            return false;
        }

        private readonly IRepository<Tracker> _trackerRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventCustomization> _eventCustomizationRepository;
    }
}