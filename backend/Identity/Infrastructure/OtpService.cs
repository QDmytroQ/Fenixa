using Identity.Domain;
using Identity.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Infrastructure
{
    public sealed class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly OtpOptions _otpOptions;
        private readonly TimeProvider _timeProvider;

        public OtpService(IOtpRepository otpRepository, IOptions<OtpOptions> otpOptions, TimeProvider timeProvider)
        {
            _otpRepository = otpRepository;
            _otpOptions = otpOptions.Value;
            _timeProvider = timeProvider;
        }

        public async Task<OtpGenerationResult> GenerateCodeAsync(Guid userId, OtpPurpose purpose, CancellationToken ct = default)
        {
            var rawCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var codeHash = HashCode(rawCode);

            var lifetime = _otpOptions.Lifetimes.TryGetValue(purpose, out var specificLifetime)
                ? specificLifetime
                : _otpOptions.DefaultLifetime;

            var expiresAt = _timeProvider.GetUtcNow().Add(lifetime);

            var otp = new Otp(userId, codeHash, purpose, expiresAt);
            await _otpRepository.SaveAsync(otp, ct);

            return new OtpGenerationResult(rawCode, expiresAt);
        }

        public async Task<bool> ValidateCodeAsync(Guid userId, string code, OtpPurpose purpose, CancellationToken ct = default)
        {
            var otp = await _otpRepository.GetAsync(userId, purpose, ct);
            if (otp == null) return false;

            if (otp.ExpiresAt < _timeProvider.GetUtcNow())
            {
                await _otpRepository.DeleteAsync(userId, purpose, ct);
                return false;
            }

            var inputHash = HashCode(code);
            if (otp.CodeHash != inputHash) return false;

            await _otpRepository.DeleteAsync(userId, purpose, ct);
            return true;
        }

        private static string HashCode(string rawCode)
        {
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawCode));
            return Convert.ToHexStringLower(hashBytes);
        }
    }
}
