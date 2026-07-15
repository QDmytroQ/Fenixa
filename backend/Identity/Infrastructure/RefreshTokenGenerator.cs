using Identity.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Infrastructure;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly RefreshTokenOptions _tokenOptions;
    public RefreshTokenGenerator(IOptions<RefreshTokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public RefreshTokenPair Generate()
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiresAt = DateTimeOffset.UtcNow.AddDays(_tokenOptions.LifetimeDays);
        return new RefreshTokenPair(rawToken, HashToken(rawToken), expiresAt);
    }

    public string HashToken(string rawToken)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexStringLower(hashBytes);
    }
}
