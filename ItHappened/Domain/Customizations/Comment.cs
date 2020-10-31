using System;

namespace ItHappened.Domain.Customizations
{
    public class Comment : IEntity, IEventCustomizationData
    {
        public Guid Id { get; }
        public Guid EventId  { get; }
        public string Content { get; }
        public DateTime CreationDate { get; }

        public Comment(Guid id, Guid eventId, string content, DateTime creationDate)
        {
            Id = id;
            EventId = eventId;
            Content = content;
            CreationDate = creationDate;
        }
    }
}