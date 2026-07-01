using MediatR;

namespace Content.Features.SearchPublicDecks;

public static class SearchPublicDecksEndpoint
{
    public static RouteGroupBuilder MapSearchPublicDecksEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/decks/public", async (
            string? searchTerm,
            string? targetLanguage,
            int page,
            int pageSize,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new SearchPublicDecksQuery(
                searchTerm,
                targetLanguage,
                page <= 0 ? 1 : page,
                pageSize <= 0 ? 20 : pageSize);

            var response = await mediator.Send(query, cancellationToken);
            return Results.Ok(response);
        });

        return group;
    }
}
