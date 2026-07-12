using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Options
{
    public class RefreshTokenOptions
    {
        public int LifetimeDays { get; set; }
    }
}
