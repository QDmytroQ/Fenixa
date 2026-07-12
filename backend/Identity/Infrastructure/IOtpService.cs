using Identity.Domain;

namespace Identity.Infrastructure
{
    public interface IOtpService
    {
        Task<OtpGenerationResult> GenerateCodeAsync(Guid userId, OtpPurpose purpose, CancellationToken ct = default);
        Task<bool> ValidateCodeAsync(Guid userId, string code, OtpPurpose purpose, CancellationToken ct = default);
    }

    public record OtpGenerationResult(string Code, DateTimeOffset ExpiresAt);
}
