using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Identity.Infrastructure
{
    public sealed class JwtProvider : IDisposable
    {
        private const int AccessTokenLifetimeMinutes = 15;

        private static readonly JwtSecurityTokenHandler _handler = new();

        private readonly RSA _rsa;
        private readonly SigningCredentials _credentials;

        private readonly string _issuer;
        private readonly string _audience;

        public JwtProvider(IOptions<CryptoOptions> options)
        {
            var cryptoOptions = options.Value;

            if (string.IsNullOrWhiteSpace(cryptoOptions.PrivateKey))
            {
                throw new InvalidOperationException("Private key is not configured.");
            }

            if (string.IsNullOrWhiteSpace(cryptoOptions.Issuer))
            {
                throw new InvalidOperationException("Issuer is not configured.");
            }

            if (string.IsNullOrWhiteSpace(cryptoOptions.Audience))
            {
                throw new InvalidOperationException("Audience is not configured.");
            }

            _issuer = cryptoOptions.Issuer;
            _audience = cryptoOptions.Audience;

            _rsa = RSA.Create();
            _rsa.ImportFromPem(cryptoOptions.PrivateKey);

            var securityKey = new RsaSecurityKey(_rsa);
            _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }

        public string GenerateAccessToken(Guid userId)
        {
            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new("tokenType", "access"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            return GenerateTokenInternal(claims, 
                DateTime.UtcNow.AddMinutes(AccessTokenLifetimeMinutes));
        }

        private string GenerateTokenInternal(Claim[] claims, DateTime expires)
        {
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expires,
                signingCredentials: _credentials);

            return _handler.WriteToken(token);
        }

        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}