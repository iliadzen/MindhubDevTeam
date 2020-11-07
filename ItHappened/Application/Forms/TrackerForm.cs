using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public class TrackerForm
    {
        public string Title { get; }
        public string Customizations { get; }

        public TrackerForm(string title, string customizations)
        {
            Title = title;
            Customizations = customizations;
        }
    }
}