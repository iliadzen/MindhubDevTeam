using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class Tracker
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Title { get; }
        public DateTime CreationDate { get; }
        public ISet<CustomizationType> Customizations { get; private set; }

        public Tracker(Guid id, Guid userId, string title,
            DateTime creationDate, ISet<CustomizationType> customizations)
        {
            Id = id;
            UserId = userId;
            Title = title;
            CreationDate = creationDate;
            Customizations = customizations;
        }
    }
}