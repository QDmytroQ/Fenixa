using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure
{
    public sealed class AccessTokenGenerator: JwtTokenGenerator, IAccessTokenGenerator
    {
        private const int AccessTokenLifetimeMinutes = 15;

        public AccessTokenGenerator(JwtProvider jwtProvider) : base(jwtProvider){ }

        public AccessToken Generate(Guid userId)
        {
            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new("tokenType", "access"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            var expires = DateTimeOffset.UtcNow.AddMinutes(AccessTokenLifetimeMinutes);
            return new AccessToken(_jwtProvider.GenerateToken(claims, expires), expires);
        }
    }
}
