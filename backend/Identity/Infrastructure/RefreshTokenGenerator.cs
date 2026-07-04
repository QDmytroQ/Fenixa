using System.Security.Cryptography;
using System.Text;

namespace Identity.Infrastructure;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly RefreshTokenOptions _tokenOptions;
    public RefreshTokenGenerator(RefreshTokenOptions tokenOptions)
    {
        _tokenOptions = tokenOptions;
    }

    public RefreshTokenPair Generate()
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expires = DateTimeOffset.UtcNow.AddDays(_tokenOptions.LifetimeDays);
        return new RefreshTokenPair(rawToken, HashToken(rawToken), expires);
    }

    public string HashToken(string rawToken)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexStringLower(hashBytes);
    }
}
