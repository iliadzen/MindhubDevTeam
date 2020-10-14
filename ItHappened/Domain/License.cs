using System;

namespace ItHappened.Domain
{
    public readonly struct License
    {
        public enum LicenseType
        {
            Free,
            Premium
        }

        public LicenseType Type { get; }
        public DateTime ExpiryDate { get; }

        public License(LicenseType type, DateTime expiryDate)
        {
            Type = type;
            ExpiryDate = expiryDate;
        }
    }
}