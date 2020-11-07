using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Tracker> _trackerRepository;

        public EventService(IRepository<Event> eventRepository, IRepository<Tracker> trackerRepository)
        {
            _eventRepository = eventRepository;
            _trackerRepository = trackerRepository;
        }

        public Guid CreateEvent(Guid actorId, Guid trackerId, EventForm eventForm)
        {
            if (eventForm.IsNull() || !eventForm.IsCorrectlyFilled()) return Guid.Empty;
            var optionTracker = _trackerRepository.Get(trackerId);
            return optionTracker.Match(
                Some: tracker =>
            {
                if (actorId != tracker.UserId) return Guid.Empty;
                var eventId = Guid.NewGuid();
                var @event = new Event(eventId, trackerId, eventForm.Title, 
                    DateTime.Now, DateTime.Now);
                _eventRepository.Save(@event);
                return eventId;
            },
                None: Guid.Empty); }
        
        public Option<Event> GetEvent(Guid actorId, Guid eventId)
        {
            var optionEvent = _eventRepository.Get(eventId);
            var @event = optionEvent.Match(
                Some: e =>
                {
                    var optionTracker = _trackerRepository.Get(e.TrackerId);
                    return optionTracker.Match(
                        Some: tracker => actorId == tracker.UserId ? optionEvent : Option<Event>.None,
                        None: Option<Event>.None);
                },
                None: Option<Event>.None);
            return @event;
        }

        public IReadOnlyCollection<Event> GetEventsByTrackerId(Guid actorId, Guid trackerId)
        {
            var optionTracker = _trackerRepository.Get(trackerId);
            return optionTracker.Match(
                Some: tracker => (tracker.UserId == actorId) 
                    ? _eventRepository
                        .GetAll()
                        .Where(@event => @event.TrackerId == trackerId)
                        .OrderBy(@event => @event.CreationDate)
                        .ToList().AsReadOnly()
                    : new ReadOnlyCollection<Event>(new List<Event>()),
                None: new ReadOnlyCollection<Event>(new List<Event>()));
        }
        
        public void EditEvent(Guid actorId, Guid eventId, EventForm eventForm)
        {
            if (eventForm.IsNull() || !eventForm.IsCorrectlyFilled()) return;
            var optionEvent = _eventRepository.Get(eventId);
            optionEvent.Do(@event =>
            {
                var optionTracker = _trackerRepository.Get(@event.TrackerId);
                optionTracker.Do(tracker =>
                {
                    if (actorId != tracker.UserId) return;
                    @event.Title = eventForm.Title;
                    @event.ModificationDate = DateTime.Now;
                    _eventRepository.Update(@event);

                });
            });
        }
        
        public void DeleteEvent(Guid actorId, Guid eventId)
        {
            var optionEvent = _eventRepository.Get(eventId);
            optionEvent.Do(@event =>
            {
                var optionTracker = _trackerRepository.Get(@event.TrackerId);
                optionTracker.Do(tracker =>
                {
                    if (actorId != tracker.UserId) return;
                    _eventRepository.Delete(eventId);
                });
            });
        }
    }
}