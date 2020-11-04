using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.App.Model
{
    public class EventGetResponse
    {
        public Guid Id { get; }
        public string Title { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }
        public List<ICustomizationGetResponse> Customizations { get; }

        public EventGetResponse(Event @event, List<ICustomizationGetResponse> customizations)
        {
            Id = @event.Id;
            Title = @event.Title;
            CreationDate = @event.CreationDate;
            ModificationDate = @event.ModificationDate;
            Customizations = customizations;
        }
    }
}