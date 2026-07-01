using Content.Features.AddCard;
using Content.Features.ClonePublicDeck;
using Content.Features.CreateDeck;
using Content.Features.DeleteCard;
using Content.Features.DeleteDeck;
using Content.Features.GetDeckCards;
using Content.Features.GetUserDecks;
using Content.Features.SearchPublicDecks;
using Content.Features.UpdateCard;
using Content.Features.UpdateDeck;
using Content.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Content;

public static class ContentModuleExtensions
{
    public static IServiceCollection AddContentModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ContentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", "content")));

        services.AddValidatorsFromAssembly(typeof(CreateDeckCommandValidator).Assembly);

        return services;
    }

    public static WebApplication MapContentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/content")
            .WithTags("Content");

        group.MapCreateDeckEndpoint();
        group.MapGetUserDecksEndpoint();
        group.MapSearchPublicDecksEndpoint();
        group.MapUpdateDeckEndpoint();
        group.MapDeleteDeckEndpoint();
        group.MapClonePublicDeckEndpoint();
        group.MapGetDeckCardsEndpoint();
        group.MapAddCardEndpoint();
        group.MapUpdateCardEndpoint();
        group.MapDeleteCardEndpoint();

        return app;
    }
}
