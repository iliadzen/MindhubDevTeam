using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Scale : IEntity, IEventCustomizationData
    {
        public Guid Id { get; }
        public Guid EventId  { get; }
        public double Value { get; }

        public Scale(Guid id, Guid eventId, double value)
        {
            Id = id;
            EventId = eventId;
            Value = value;
        }
    }
}