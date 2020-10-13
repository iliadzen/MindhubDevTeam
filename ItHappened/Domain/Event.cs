using System;

namespace ItHappened.Domain
{
    public class Event
    {
        public Guid Id { get; }
        public Guid TrackerId { get; }
        public string Title { get; private set; }
        public DateTime CreationDate { get; }
        public DateTime ChangingDate { get; private set; }

        public Event(Guid id, Guid trackerId, string title, DateTime creationDate, DateTime changingDate)
        {
            Id = id;
            TrackerId = trackerId;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            CreationDate = creationDate;
            ChangingDate = changingDate;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }
        
        public void SetChangingDate(DateTime changingDate)
        {
            ChangingDate = changingDate;
        }
    }
}