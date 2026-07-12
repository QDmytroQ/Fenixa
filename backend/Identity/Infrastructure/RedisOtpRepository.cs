using Identity.Domain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Identity.Infrastructure
{
    public class RedisOtpRepository : IOtpRepository
    {
        private readonly IDistributedCache _cache;
        public RedisOtpRepository(IDistributedCache cache) 
        {
            _cache = cache;
        }

        public async Task SaveAsync(Otp otp, CancellationToken cancellationToken = default)
        {
            var key = GenerateKey(otp.UserId, otp.Purpose);

            var json = JsonSerializer.Serialize(otp);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = otp.ExpiresAt
            };

            await _cache.SetStringAsync(key, json, options, cancellationToken);
        }

        public async Task DeleteAsync(Guid userId, OtpPurpose purpose, CancellationToken cancellationToken = default)
        {
            var key = GenerateKey(userId, purpose);

            await _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task<Otp?> GetAsync(Guid userId, OtpPurpose purpose, CancellationToken cancellationToken = default)
        {
            var key = GenerateKey(userId, purpose);

            var json = await _cache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Otp>(json);
        }

        private static string GenerateKey(Guid userId, OtpPurpose purpose)
        {
            return $"identity:otp:{userId}:{purpose}";
        }
    }
}
