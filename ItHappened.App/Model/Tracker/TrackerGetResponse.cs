using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.App.Model
{
    public class TrackerGetResponse
    {
        public Guid Id { get; }
        public string Title { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }
        //public ISet<CustomizationType> Customizations { get; }

        public TrackerGetResponse(Tracker tracker)
        {
            Id = tracker.Id;
            Title = tracker.Title;
            CreationDate = tracker.CreationDate;
            ModificationDate = tracker.ModificationDate;
            //Customizations = tracker.Customizations;
        }
    }
}