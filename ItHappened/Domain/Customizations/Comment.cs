using System;

namespace ItHappened.Domain.Customizations
{
    public class Comment : IEntity, IEventCustomizationData
    {
        public Guid Id { get; private set; }
        public Guid EventId  { get; private set; }
        public string Content { get; private set; }

        public Comment(Guid id, Guid eventId, string content)
        {
            Id = id;
            EventId = eventId;
            Content = content;
        }

        public Comment()
        {
        }
    }
}