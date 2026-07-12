

namespace Identity.Infrastructure
{
    public interface IEmailVerificationCookieWriter
    {
        void Append(string token, DateTimeOffset expiresAt);

        void Clear();
    }
}
