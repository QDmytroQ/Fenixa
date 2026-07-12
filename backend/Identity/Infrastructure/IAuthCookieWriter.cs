namespace Identity.Infrastructure;

public interface IAuthCookieWriter
{
    void Append(string accessToken, DateTimeOffset accessExpires, string refreshToken, DateTimeOffset refreshExpires);

    void Clear();
}
