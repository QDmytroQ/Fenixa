namespace Identity.Infrastructure;

public sealed record RefreshTokenPair(string RawToken, string TokenHash);

public interface IRefreshTokenGenerator
{
    RefreshTokenPair Generate();
    string HashToken(string rawToken);
}
