using Identity.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Options
{
    public class OtpOptions
    {
        public Dictionary<OtpPurpose, TimeSpan> Lifetimes { get; set; } = new();

        public TimeSpan DefaultLifetime { get; set; } = TimeSpan.FromMinutes(5);
    }
}
