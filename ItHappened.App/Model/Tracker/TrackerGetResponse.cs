using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.App.Model
{
    public class TrackerGetResponse
    {
        public Guid Id { get; }
        public string Title { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }
        //public List<CustomizationType> Customizations { get; }
        public List<string> Customizations { get; } = new List<string>();

        public TrackerGetResponse(Tracker tracker)
        {
            Id = tracker.Id;
            Title = tracker.Title;
            CreationDate = tracker.CreationDate;
            ModificationDate = tracker.ModificationDate;
            foreach (var customization in tracker.Customizations)
                Customizations.Add(customization.ToString());
        }
    }
}