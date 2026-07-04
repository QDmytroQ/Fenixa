using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Identity.Infrastructure;

public sealed class AuthCookieService : IAuthCookieService
{
    public const string AccessTokenCookieName = "access-token";
    public const string RefreshTokenCookieName = "refresh-token";

    private const int AccessTokenLifetimeMinutes = 15;

    private readonly IWebHostEnvironment _environment;

    public AuthCookieService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public void SetAuthCookies(
        HttpContext context,
        string accessToken,
        string refreshToken,
        DateTimeOffset refreshExpires)
    {
        var sameSite = _environment.IsDevelopment()
            ? SameSiteMode.None
            : SameSiteMode.Lax;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = sameSite,
            Path = "/"
        };

        context.Response.Cookies.Append(
            AccessTokenCookieName,
            accessToken,
            new CookieOptions
            {
                HttpOnly = cookieOptions.HttpOnly,
                Secure = cookieOptions.Secure,
                SameSite = cookieOptions.SameSite,
                Path = cookieOptions.Path,
                Expires = DateTimeOffset.UtcNow.AddMinutes(AccessTokenLifetimeMinutes)
            });

        context.Response.Cookies.Append(
            RefreshTokenCookieName,
            refreshToken,
            new CookieOptions
            {
                HttpOnly = cookieOptions.HttpOnly,
                Secure = cookieOptions.Secure,
                SameSite = cookieOptions.SameSite,
                Path = cookieOptions.Path,
                Expires = refreshExpires
            });
    }
}
