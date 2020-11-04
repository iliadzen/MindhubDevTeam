using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Geotag : IEntity, IEventCustomizationData
    {
        public Guid Id { get; private  set; }
        public Guid EventId  { get; private  set; }
        public decimal Longitude { get; private  set; }
        public decimal Latitude { get; private  set; }

        public Geotag(Guid id, Guid eventId, decimal longitude, decimal latitude)
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