using Identity.Domain;

namespace Identity.Infrastructure
{
    public interface IOtpRepository
    {
        Task SaveAsync(Otp otp, CancellationToken cancellationToken = default);
        Task<Otp?> GetAsync(Guid userId, OtpPurpose purpose, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid userId, OtpPurpose purpose, CancellationToken cancellationToken = default);
    }
}
