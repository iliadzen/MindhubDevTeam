using System;

namespace ItHappened.App.Authentication
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        
        public TimeSpan ExpiresAfter { get; set; }
    }
}