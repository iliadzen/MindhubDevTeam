using System;
using ItHappened.Domain.Customizations;

namespace ItHappened.Domain.Customizations
{
    public class Photo : IEntity, IEventCustomizationData
    {
        public Guid Id { get; private  set;}
        public Guid EventId  { get; private  set; }
        public string Image { get; private  set; }

        public Photo(Guid id, Guid eventId, string image)
        {
            Id = id;
            EventId = eventId;
            Image = image;
        }

        public Photo()
        {
        }
    }
}