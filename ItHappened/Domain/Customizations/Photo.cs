using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Photo : IEntity, IEventCustomizationData
    {
        public Guid Id { get; }
        public Guid EventId  { get; }
        public byte[] File { get;  }

        public Photo(Guid id, Guid eventId, string name, byte[] file)
        {
            Id = id;
            EventId = eventId;
            File = file;
        }
    }
}