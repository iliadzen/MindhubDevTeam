using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Rating : IEntity, IEventCustomizationData
    {
        public enum StarsRating
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }
        
        public Guid Id { get; }
        public Guid EventId  { get; }
        public StarsRating Stars { get; }

        public Rating(Guid id, Guid eventId, StarsRating stars)
        {
            Id = id;
            EventId = eventId;
            Stars = stars;
        }
    }
}