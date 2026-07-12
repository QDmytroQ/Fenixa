using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure
{
    public sealed class EmailVerificationTokenGenerator : JwtTokenGenerator, IEmailVerificationTokenGenerator
    {

        public EmailVerificationTokenGenerator(JwtProvider jwtProvider): base(jwtProvider) { }

        public string Generate(Guid userId, DateTimeOffset expiresAt)
        {
            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new("tokenType", "email_verification")
            ];

            return _jwtProvider.GenerateToken(claims, expiresAt);
        }
    }
}
