using System;

namespace ItHappened.Domain
{
    public interface IEventRepository
    {
        Event Load(Guid eventId);
        void Save(Event @event);
        void Update(Event @event);
        void Delete(Guid eventId);
    }
}