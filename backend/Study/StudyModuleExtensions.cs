using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Study.Features.GetStudySession;
using Study.Features.GetStudyStatistics;
using Study.Features.LogDailyActivity;
using Study.Features.ReviewCard;
using Study.Persistence;

namespace Study;

public static class StudyModuleExtensions
{
    public static IServiceCollection AddStudyModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<StudyDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "study")));

        services.AddValidatorsFromAssembly(typeof(ReviewCardCommandValidator).Assembly);

        return services;
    }

    public static WebApplication MapStudyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/study")
            .WithTags("Study");

        group.MapGetStudySessionEndpoint();
        group.MapReviewCardEndpoint();
        group.MapLogDailyActivityEndpoint();
        group.MapGetStudyStatisticsEndpoint();

        return app;
    }
}
