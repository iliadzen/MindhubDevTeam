using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public class TrackerCreationForm
    {
        public string Title { get; }
        public ISet<CustomizationType> Customizations { get; }

        public TrackerCreationForm(string title, ISet<CustomizationType> customizations)
        {
            Title = title;
            Customizations = customizations;
        }
    }
}