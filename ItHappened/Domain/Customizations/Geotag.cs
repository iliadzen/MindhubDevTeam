using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Geotag : IEntity, IEventCustomizationData
    {
        public Guid Id { get; private  set; }
        public Guid EventId  { get; private  set; }
        public double Longitude { get; private  set; }
        public double Latitude { get; private  set; }

        public Geotag(Guid id, Guid eventId, double longitude, double latitude)
        {
            Id = id;
            EventId = eventId;
            Longitude = longitude;
            Latitude = latitude;
        }

        public Geotag()
        {
        }
    }
}