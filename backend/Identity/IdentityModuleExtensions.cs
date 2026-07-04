using FluentValidation;
using Identity.Features.LoginUser;
using Identity.Features.RefreshToken;
using Identity.Features.RegisterUser;
using Identity.Features.UpdateGeminiKey;
using Identity.Infrastructure;
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
        services.AddSingleton(Microsoft.Extensions.Options.Options.Create(cryptoOptions));
        services.AddSingleton<JwtProvider>();
        services.AddScoped<IGeminiKeyEncryptor, GeminiKeyEncryptor>();
        services.AddScoped<IUserPasswordHasher, UserPasswordHasher>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IAuthCookieService, AuthCookieService>();

        services.AddValidatorsFromAssembly(typeof(RegisterUserCommandValidator).Assembly);

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

        return app;
    }
}
