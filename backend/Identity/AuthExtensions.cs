using Identity.Infrastructure;
using Identity.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Identity
{
    public static class ApiExstensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, CryptoOptions cryptoOptions)
        {
            const string defaultScheme = JwtBearerDefaults.AuthenticationScheme;
            const string emailVerificationScheme = "EmailVerificationScheme";
            const string TwoFactorAuthScheme = "TwoFactorAuthScheme";

            var rsa = RSA.Create();
            rsa.ImportFromPem(cryptoOptions.PublicKey);
            var securityKey = new RsaSecurityKey(rsa);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = cryptoOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = cryptoOptions.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidAlgorithms = [SecurityAlgorithms.RsaSha256]
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = defaultScheme;
                options.DefaultChallengeScheme = defaultScheme;
            })
            .AddJwtBearer(defaultScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[IdentityCookieWriter.AccessTokenCookieName];
                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(emailVerificationScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[IdentityCookieWriter.EmailVerificationCookieName];
                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(TwoFactorAuthScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[IdentityCookieWriter.TwoFactorAuthCookieName];
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(defaultScheme)
                    .RequireAuthenticatedUser()
                    .RequireClaim("tokenType", "access")
                    .Build();

                options.DefaultPolicy = defaultPolicy;

                options.AddPolicy("EmailVerificationOnly", policy =>
                    policy.AddAuthenticationSchemes(emailVerificationScheme)
                          .RequireAuthenticatedUser()
                          .RequireClaim("tokenType", "email_verification"));

                options.AddPolicy("TwoFactorAuthOnly", policy =>
                    policy.AddAuthenticationSchemes(TwoFactorAuthScheme)
                          .RequireAuthenticatedUser()
                          .RequireClaim("tokenType", "two_factor_auth"));
            });

            return services;
        }
    }
}