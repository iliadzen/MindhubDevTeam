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
        
        public Guid Id { get; private set; }
        public Guid EventId  { get; private set; }
        public StarsRating Stars { get; private set; }

        public Rating(Guid id, Guid eventId, StarsRating stars)
        {
            Id = id;
            EventId = eventId;
            Stars = stars;
        }

        public Rating()
        {
        }
    }
}