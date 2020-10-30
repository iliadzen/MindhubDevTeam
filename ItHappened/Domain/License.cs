using System;

namespace ItHappened.Domain
{
    public class License
    {
        public LicenseType Type { get; set; }
        public DateTime ExpiryDate { get; set; }

        public License(LicenseType type, DateTime expiryDate)
        {
            Type = type;
            ExpiryDate = expiryDate;
        }
        
        public License()
        {
            Type = LicenseType.Free;
            ExpiryDate = DateTime.Now;
        }
    }
}