using System;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public class EventRepository : IEventRepository
    {
        public Event Load(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public void Save(Event @event)
        {
            throw new NotImplementedException();
        }

        public void Update(Event @event)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid eventId)
        {
            throw new NotImplementedException();
        }
    }
}