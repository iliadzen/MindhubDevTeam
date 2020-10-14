using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public class TrackerEditingContent
    {
        public string Title { get; }
        public ISet<CustomizationType> Customizations { get; }

        public TrackerEditingContent(Guid userId, string title, ISet<CustomizationType> customizations)
        {
            Title = title;
            Customizations = customizations;
        }
    }
}