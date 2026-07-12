using FluentValidation;
using Identity.Domain;
using Identity.Features.LoginUser;
using Identity.Features.LogOut;
using Identity.Features.RefreshToken;
using Identity.Features.RegisterUser;
using Identity.Features.TwoFactorAuth;
using Identity.Features.UpdateGeminiKey;
using Identity.Features.VerifyEmail;
using Identity.Infrastructure;
using Identity.Infrastructure.Options;
using Identity.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity;

public static class IdentityModuleExtensions
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration,
        CryptoOptions cryptoOptions)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "identity")));

        services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshToken"));
        services.Configure<AccessTokenOptions>(configuration.GetSection("AccessToken"));
        services.Configure<OtpOptions>(options =>
        {
            options.Lifetimes = new Dictionary<OtpPurpose, TimeSpan>
            {
                { OtpPurpose.TwoFactorAuth, TimeSpan.FromMinutes(5) },
                { OtpPurpose.PasswordReset, TimeSpan.FromMinutes(15) },
                { OtpPurpose.EmailVerification, TimeSpan.FromHours(1) }
            };
            options.DefaultLifetime = TimeSpan.FromMinutes(5);
        });
        services.AddSingleton(Microsoft.Extensions.Options.Options.Create(cryptoOptions));
        services.AddSingleton<JwtProvider>();

        services.AddScoped<IdentityCookieWriter>();
        services.AddScoped<IAuthCookieWriter>(sp => sp.GetRequiredService<IdentityCookieWriter>());
        services.AddScoped<IEmailVerificationCookieWriter>(sp => sp.GetRequiredService<IdentityCookieWriter>());
        services.AddScoped<ITwoFactorAuthCookieWriter>(sp => sp.GetRequiredService<IdentityCookieWriter>());
        services.AddScoped<IGeminiKeyEncryptor, GeminiKeyEncryptor>();
        services.AddScoped<IUserPasswordHasher, UserPasswordHasher>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<ITwoFactorTokenGenerator, TwoFactorTokenGenerator>();
        services.AddScoped<IEmailVerificationTokenGenerator, EmailVerificationTokenGenerator>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IOtpRepository, RedisOtpRepository>();

        services.AddValidatorsFromAssembly(typeof(RegisterUserCommandValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(LoginUserCommandValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(VerifyEmailCommandValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(TwoFactorAuthCommandValidator).Assembly);

        services.AddApiAuthentication(cryptoOptions);

        return services;
    }

    public static WebApplication MapIdentityEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/identity")
            .WithTags("Identity");

        group.MapRegisterUserEndpoint();
        group.MapLoginUserEndpoint();
        group.MapRefreshTokenEndpoint();
        group.MapUpdateGeminiKeyEndpoint();
        group.MapLogoutUserEndpoint();

        return app;
    }
}
