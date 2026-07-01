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
            ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var query = new GetDeckCardsQuery(currentUser.UserId.Value, deckId);
            var response = await mediator.Send(query, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
