using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application
{
    public interface IEventService
    {
        Guid CreateEvent(Guid actorId, Guid trackerId, EventForm eventForm);
        Option<Event> GetEvent(Guid actorId, Guid eventId);
        IReadOnlyCollection<Event> GetEventsByTrackerId(Guid actorId, Guid trackerId);
        void EditEvent(Guid actorId, Guid eventId, EventForm eventForm);
        void DeleteEvent(Guid actorId, Guid eventId);
    }
}