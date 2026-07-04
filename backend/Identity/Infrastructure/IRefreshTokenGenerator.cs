namespace Identity.Infrastructure;

public sealed record RefreshTokenPair(string RawToken, string TokenHash, DateTimeOffset Expires);

public interface IRefreshTokenGenerator
{
    RefreshTokenPair Generate();
    string HashToken(string rawToken);
}
