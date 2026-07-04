using MediatR;
using Shared.Abstractions;

namespace Content.Features.DeleteDeck;

public static class DeleteDeckEndpoint
{
    public static RouteGroupBuilder MapDeleteDeckEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/decks/{deckId:guid}", async (
            Guid deckId,
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new DeleteDeckCommand(currentUser.UserId.Value, deckId);
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization();

        return group;
    }
}
