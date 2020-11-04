using System;

namespace ItHappened.Domain
{
    public class License : IEntity
    {
        public Guid Id {get; private set;  }
        public Guid UserId {get; private set;  }
        public LicenseType Type { get; set;  }
        public DateTime ExpiryDate { get; set;  }

        public License(Guid id, Guid userId, LicenseType type, DateTime expiryDate)
        {
            Id = id;
            UserId = userId;
            Type = type;
            ExpiryDate = expiryDate;
        }
        
        public License()
        {
        }
    }
}