using System;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public class EventService
    {
        public IEventRepository EventRepository { get; }

        public EventService(IEventRepository eventRepository)
        {
            EventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        }

        public Guid CreateEvent(Guid actorId, Guid trackerId, string title)
        {
            // What we will do if user can't create event?
            if (!CanCreateEvent(actorId)) throw new NotImplementedException();
            if (title == null) throw new ArgumentNullException(nameof(title));
            var eventId = Guid.NewGuid();
            var @event = new Event(eventId, trackerId, title, DateTime.Now, DateTime.Now);
            EventRepository.Save(@event);
            return eventId;
        }
        
        public Event GetEvent(Guid actorId, Guid eventId)
        {
            // What we will do if user can't get event?
            if (!CanGetEvent(actorId, eventId)) throw new NotImplementedException();
            var @event = EventRepository.Load(eventId);
            return @event;
        }
        
        public void EditEvent(Guid actorId, Guid eventId, string title)
        {
            // What we will do if user can't edit event?
            if (!CanEditEvent(actorId, eventId)) throw new NotImplementedException();
            var @event = GetEvent(actorId, eventId);
            @event.Title = title;
            @event.ModificationDate = DateTime.Now;
            EventRepository.Update(@event);
        }
        
        public void DeleteEvent(Guid actorId, Guid eventId)
        {
            if (!CanDeleteEvent(actorId, eventId)) throw new NotImplementedException();
            EventRepository.Delete(eventId);
            // Cascade dependencies (CustomEventData) delete
        }

        // Permission methods
        // WIP: After other models will be created
        private bool CanCreateEvent(Guid userId)
        {
            throw new NotImplementedException();
        }
        
        private bool CanGetEvent(Guid userId, Guid eventId)
        {
            throw new NotImplementedException();
        }
        
        private bool CanEditEvent(Guid userId, Guid eventId)
        {
            throw new NotImplementedException();
        }
        
        private bool CanDeleteEvent(Guid userId, Guid eventId)
        {
            throw new NotImplementedException();
        }
    }
}