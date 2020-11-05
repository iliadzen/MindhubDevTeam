using System;
using System.Collections.Generic;
using ItHappened.Infrastructure;

namespace ItHappened.Domain
{
    public class Tracker : IEntity
    {
        public Guid Id { get; private set;  }
        public Guid UserId { get; private set;  }
        public string Title { get; set;  }
        public DateTime CreationDate { get; private set;  }
        public DateTime ModificationDate { get; set;  }
        public List<CustomizationType> Customizations { get; set; }

        public Tracker(Guid id, Guid userId, string title, DateTime creationDate, 
            DateTime modificationDate, List<CustomizationType> customizations)
        {
            Id = id;
            UserId = userId;
            Title = title;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
            Customizations = customizations;
        }

        public Tracker()
        {
        }
    }
}