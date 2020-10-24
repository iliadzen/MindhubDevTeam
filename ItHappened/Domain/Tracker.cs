using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public class Tracker : IEntity
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Title { get; }
        public DateTime CreationDate { get; }
        public DateTime ModificationDate { get; }
        public ISet<CustomizationType> Customizations { get; }

        public Tracker(Guid id, Guid userId, string title, DateTime creationDate, 
            DateTime modificationDate, ISet<CustomizationType> customizations)
        {
            Id = id;
            UserId = userId;
            Title = title;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
            Customizations = customizations;
        }
    }
}