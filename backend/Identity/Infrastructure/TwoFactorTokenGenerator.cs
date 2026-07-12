using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Identity.Infrastructure
{
    public sealed class TwoFactorTokenGenerator : JwtTokenGenerator, ITwoFactorTokenGenerator
    {
        public TwoFactorTokenGenerator(JwtProvider jwtProvider) : base(jwtProvider){ }
        public string Generate(Guid userId, DateTimeOffset expiresAt)
        {
            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new("tokenType", "two_factor_auth"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];
            return _jwtProvider.GenerateToken(claims, expiresAt);
        }
    }
}
