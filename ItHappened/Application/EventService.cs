using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Application
{
    public class EventService : IEventService
    {
        private IRepository<Event> EventRepository { get; }
        private IRepository<Tracker> TrackerRepository { get; }

        public EventService(IRepository<Event> eventRepository, IRepository<Tracker> trackerRepository)
        {
            EventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            TrackerRepository = trackerRepository ?? throw new ArgumentNullException(nameof(trackerRepository));
        }

        public void CreateEvent(Guid actorId, Guid trackerId, EventContent eventContent)
        {
            if (eventContent.IsNull()) return;
            var optionTracker = TrackerRepository.Get(trackerId);
            optionTracker.Do(tracker =>
            {
                if (actorId != tracker.UserId) return;
                var eventId = Guid.NewGuid();
                var @event = new Event(eventId, trackerId, eventContent.Title, DateTime.Now, DateTime.Now);
                EventRepository.Save(@event);
            });
        }
        
        public Option<Event> GetEvent(Guid actorId, Guid eventId)
        {
            var optionEvent = EventRepository.Get(eventId);
            return optionEvent.Match(
                Some: @event =>
                {
                    var optionTracker = TrackerRepository.Get(@event.TrackerId);
                    return optionTracker.Match(
                        Some: tracker => actorId == tracker.UserId ? Option<Event>.None : optionEvent,
                        None: Option<Event>.None);
                },
                None: Option<Event>.None);
        }

        public IReadOnlyCollection<Event> GetEventsByTrackerId(Guid trackerId)
        {
            return (IReadOnlyCollection<Event>) EventRepository.GetAll().Where(@event =>
                @event.TrackerId == trackerId);
        }
        
        public void EditEvent(Guid actorId, Guid eventId, EventContent eventContent)
        {
            if (eventContent.IsNull()) return;
            var optionEvent = EventRepository.Get(eventId);
            optionEvent.Do(@event =>
            {
                var optionTracker = TrackerRepository.Get(@event.TrackerId);
                optionTracker.Do(tracker =>
                {
                    if (actorId != tracker.UserId) return;
                    @event.Title = eventContent.Title;
                    @event.ModificationDate = DateTime.Now;
                    EventRepository.Update(@event);

                });
            });
        }
        
        public void DeleteEvent(Guid actorId, Guid eventId)
        {
            var optionEvent = EventRepository.Get(eventId);
            optionEvent.Do(@event =>
            {
                var optionTracker = TrackerRepository.Get(@event.TrackerId);
                optionTracker.Do(tracker =>
                {
                    if (actorId != tracker.UserId) return;
                    EventRepository.Delete(eventId);
                    // Cascade dependencies (CustomEventData) delete:
                    // GetCustomEventDataByEvent
                    // foreach delete
                });
            });
        }
    }
}