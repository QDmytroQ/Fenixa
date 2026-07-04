using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure
{
    public class AuthOptions
    {
        public int AccessTokenLifetimeMinutes { get; set; }
        public int RefreshTokenLifetimeDays { get; set; }
    }
}
