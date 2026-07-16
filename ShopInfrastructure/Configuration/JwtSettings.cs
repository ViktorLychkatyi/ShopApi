using System;
using System.Collections.Generic;
using System.Text;

namespace ShopInfrastructure.Configuration
{
    public class JwtSettings 
    { 
        public string Key { get; set; } = null!; 
        public string Issuer { get; set; } = null!; 
        public string Audience { get; set; } = null!; 
        public int ExpiresMinutes { get; set; }
        public int ExpiresRefreshTokenDay { get; set; }
    }
}
