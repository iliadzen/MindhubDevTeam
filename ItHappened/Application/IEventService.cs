using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application
{
    public interface IEventService
    {
        Guid CreateEvent(Guid actorId, Guid trackerId, EventContent eventContent);
        Option<Event> GetEvent(Guid actorId, Guid eventId);
        IReadOnlyCollection<Event> GetEventsByTrackerId(Guid actorId, Guid trackerId);
        void EditEvent(Guid actorId, Guid eventId, EventContent eventContent);
        void DeleteEvent(Guid actorId, Guid eventId);
    }
}