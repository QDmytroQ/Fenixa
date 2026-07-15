using MediatR;
using Shared.Abstractions;

namespace Content.Features.GetDeckCards;

public static class GetDeckCardsEndpoint
{
    public static RouteGroupBuilder MapGetDeckCardsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/decks/{deckId:guid}/cards", async (
            Guid deckId,
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var query = new GetDeckCardsQuery(currentUser.UserId, deckId);
            var response = await mediator.Send(query, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
