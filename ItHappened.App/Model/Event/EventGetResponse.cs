using System;
using ItHappened.Domain;

namespace ItHappened.App.Model
{
    public class EventGetResponse
    {
        public Guid Id { get; }
        public string Title { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }

        public EventGetResponse(Event @event)
        {
            Id = @event.Id;
            Title = @event.Title;
            CreationDate = @event.CreationDate;
            ModificationDate = @event.ModificationDate;
        }
    }
}