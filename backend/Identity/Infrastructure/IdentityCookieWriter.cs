using Identity.Infrastructure;
using Microsoft.AspNetCore.Hosting;

public sealed class IdentityCookieWriter : CookieWriter,
    IAuthCookieWriter,
    IEmailVerificationCookieWriter,
    ITwoFactorAuthCookieWriter
{
    public const string AccessTokenCookieName = "access-token";
    public const string RefreshTokenCookieName = "refresh-token";
    public const string EmailVerificationCookieName = "email-verification-token";
    public const string TwoFactorAuthCookieName = "two-factor-token";

    public IdentityCookieWriter(IWebHostEnvironment env, IHttpContextAccessor accessor)
        : base(env, accessor) { }

    public void Append(string accessToken, DateTimeOffset accessExpiresAt, string refreshToken, DateTimeOffset refreshExpiresAt)
    {
        AppendCookie(AccessTokenCookieName, accessToken, accessExpiresAt);
        AppendCookie(RefreshTokenCookieName, refreshToken, refreshExpiresAt);
    }

    void IAuthCookieWriter.Clear()
    {
        DeleteCookie(AccessTokenCookieName);
        DeleteCookie(RefreshTokenCookieName);
    }

    void IEmailVerificationCookieWriter.Append(string token, DateTimeOffset expiresAt) =>
        AppendCookie(EmailVerificationCookieName, token, expiresAt);

    void IEmailVerificationCookieWriter.Clear() =>
        DeleteCookie(EmailVerificationCookieName);

    void ITwoFactorAuthCookieWriter.Append(string token, DateTimeOffset expiresAt) =>
        AppendCookie(TwoFactorAuthCookieName, token, expiresAt);

    void ITwoFactorAuthCookieWriter.Clear() =>
        DeleteCookie(TwoFactorAuthCookieName);
}