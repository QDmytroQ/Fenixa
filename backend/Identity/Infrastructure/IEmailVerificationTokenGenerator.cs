using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure
{
    internal interface IEmailVerificationTokenGenerator
    {
        string Generate(Guid userId, DateTimeOffset expiresAt);
    }
}
