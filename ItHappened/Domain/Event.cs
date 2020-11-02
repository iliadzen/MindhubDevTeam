using System;

namespace ItHappened.Domain
{
    public class Event : IEntity
    {
        public Guid Id { get; private set; }
        public Guid TrackerId { get; private set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; private set; }
        public DateTime ModificationDate { get; set; }

        public Event(Guid id, Guid trackerId, string title, DateTime creationDate, DateTime modificationDate)
        {
            Id = id;
            TrackerId = trackerId;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            CreationDate = creationDate;
            ModificationDate = modificationDate;
        }

        public Event()
        {
        }
    }
}