using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application
{
    public interface IEventService
    {
        void CreateEvent(Guid actorId, Guid trackerId, string title);
        Option<Event> GetEvent(Guid actorId, Guid eventId);
        IEnumerable<Event> GetEventsByTrackerId(Guid trackerId);
        void EditEvent(Guid actorId, Guid eventId, string title);
        void DeleteEvent(Guid actorId, Guid eventId);
    }
}