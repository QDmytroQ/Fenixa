namespace Identity.Infrastructure;

public interface IAuthCookieService
{
    void SetAuthCookies(HttpContext context, string accessToken, string refreshToken, DateTimeOffset refreshExpires);
}
