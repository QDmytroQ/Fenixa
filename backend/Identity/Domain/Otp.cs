namespace Identity.Domain
{
    public sealed record Otp(
        Guid UserId,
        string CodeHash,
        OtpPurpose Purpose,
        DateTimeOffset ExpiresAt
    );
}
