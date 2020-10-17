using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application
{
    public class TrackerCreationContent
    {
        public Guid UserId { get; }
        public string Title { get; }
        public ISet<CustomizationType> Customizations { get; }

        public TrackerCreationContent(Guid userId, string title, ISet<CustomizationType> customizations)
        {
            UserId = userId;
            Title = title;
            Customizations = customizations;
        }
    }
}