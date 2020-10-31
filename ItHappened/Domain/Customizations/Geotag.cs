using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Geotag : IEntity, IEventCustomizationData
    {
        public Guid Id { get; }
        public Guid EventId  { get; }
        public double Longitude { get; }
        public double Latitude { get; }

        public Geotag(Guid id, Guid eventId, double longitude, double latitude)
        {
            Id = id;
            EventId = eventId;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}