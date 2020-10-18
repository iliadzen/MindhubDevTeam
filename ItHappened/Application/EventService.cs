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

        public void CreateEvent(Guid actorId, Guid trackerId, string title)
        {
            if (!CanCreateEvent(actorId)) return;
            if (title == null) throw new ArgumentNullException(nameof(title));
            var eventId = Guid.NewGuid();
            var @event = new Event(eventId, trackerId, title, DateTime.Now, DateTime.Now);
            EventRepository.Save(@event);
        }
        
        public Option<Event> GetEvent(Guid actorId, Guid eventId)
        {
            if (!CanGetEvent(actorId, eventId)) return Option<Event>.None;
            var @event = EventRepository.Get(eventId);
            return @event;
        }

        public IEnumerable<Event> GetEventsByTrackerId(Guid trackerId)
        {
            return EventRepository.GetAll().Where(@event =>
                @event.TrackerId == trackerId);
        }
        
        public void EditEvent(Guid actorId, Guid eventId, string title)
        {
            if (!CanEditEvent(actorId, eventId)) return;
            var @event = GetEvent(actorId, eventId);
            @event.Do(@event =>
            {
                @event.Title = title;
                @event.ModificationDate = DateTime.Now;
                EventRepository.Update(@event);
            });
        }
        
        public void DeleteEvent(Guid actorId, Guid eventId)
        {
            if (!CanDeleteEvent(actorId, eventId)) return;
            EventRepository.Delete(eventId);
            // Cascade dependencies (CustomEventData) delete:
            // GetCustomEventDataByEvent
            // foreach delete
        }

        private bool CanCreateEvent(Guid userId)
        {
            return true;
        }
        
        private bool CanGetEvent(Guid userId, Guid eventId)
        {
            var @event = EventRepository.Get(eventId);
            if (@event.IsNone) return false;
            var tracker = TrackerRepository.Get(@event.ValueUnsafe().TrackerId);
            if (tracker.IsNone) return false;
            return userId == tracker.ValueUnsafe().UserId;
        }
        
        private bool CanEditEvent(Guid userId, Guid eventId)
        {
            var @event = EventRepository.Get(eventId);
            if (@event.IsNone) return false;
            var tracker = TrackerRepository.Get(@event.ValueUnsafe().TrackerId);
            if (tracker.IsNone) return false;
            return userId == tracker.ValueUnsafe().UserId;
        }
        
        private bool CanDeleteEvent(Guid userId, Guid eventId)
        {
            var @event = EventRepository.Get(eventId);
            if (@event.IsNone) return false;
            var tracker = TrackerRepository.Get(@event.ValueUnsafe().TrackerId);
            if (tracker.IsNone) return false;
            return userId == tracker.ValueUnsafe().UserId;
        }
    }
}