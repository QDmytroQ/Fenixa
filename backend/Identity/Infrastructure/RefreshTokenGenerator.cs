using System.Security.Cryptography;
using System.Text;

namespace Identity.Infrastructure;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public RefreshTokenPair Generate()
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return new RefreshTokenPair(rawToken, HashToken(rawToken));
    }

    public string HashToken(string rawToken)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexStringLower(hashBytes);
    }
}
