using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Scale : IEntity, IEventCustomizationData
    {
        public Guid Id { get; private  set;}
        public Guid EventId  { get; private  set; }
        public decimal Value { get; private  set; }

        public Scale(Guid id, Guid eventId, decimal value)
        {
            Id = id;
            EventId = eventId;
            Value = value;
        }

        public Scale()
        {
        }
    }
}