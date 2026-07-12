using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure
{
    public sealed record AccessToken(string Token, DateTimeOffset Expires);
    public interface IAccessTokenGenerator
    {
        AccessToken Generate(Guid userId);
    }
}
