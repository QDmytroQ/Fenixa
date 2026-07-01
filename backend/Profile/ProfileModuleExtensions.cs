using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Profile.Features.GetUserSettings;
using Profile.Features.UpdateSettings;
using Profile.Persistence;

namespace Profile;

public static class ProfileModuleExtensions
{
    public static IServiceCollection AddProfileModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProfileDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "profile")));

        services.AddValidatorsFromAssembly(typeof(UpdateSettingsCommandValidator).Assembly);

        return services;
    }

    public static WebApplication MapProfileEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/profile")
            .WithTags("Profile");

        group.MapGetUserSettingsEndpoint();
        group.MapUpdateSettingsEndpoint();

        return app;
    }
}
