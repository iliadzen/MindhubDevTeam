using System;

namespace ItHappened.Domain
{
    public class Event : IEntity
    {
        public Guid Id { get; }
        public Guid TrackerId { get; }
        public string Title { get; set; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; set; }

        public Event(Guid id, Guid trackerId, string title, DateTime creationDate, DateTime modificationDate)
        {
            Id = id;
            TrackerId = trackerId;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            CreationDate = creationDate;
            ModificationDate = modificationDate;
        }
    }
}