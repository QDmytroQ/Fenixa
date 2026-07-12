namespace Identity.Infrastructure
{
    public interface ITwoFactorAuthCookieWriter
    {
        void Append(string token, DateTimeOffset expiresAt);
        void Clear();
    }
}
