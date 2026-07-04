using Identity.Infrastructure;
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
            
            var rsa = RSA.Create();
            rsa.ImportFromPem(cryptoOptions.PublicKey);
            var securityKey = new RsaSecurityKey(rsa);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = defaultScheme;
                options.DefaultChallengeScheme = defaultScheme;
            })
            .AddJwtBearer(defaultScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access-token"];
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
            });

            return services;
        }
    }
}