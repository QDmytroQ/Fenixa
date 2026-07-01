using MediatR;
using Shared.Abstractions;

namespace Content.Features.DeleteCard;

public static class DeleteCardEndpoint
{
    public static RouteGroupBuilder MapDeleteCardEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/decks/{deckId:guid}/cards/{cardId:guid}", async (
            Guid deckId,
            Guid cardId,
            IMediator mediator,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new DeleteCardCommand(currentUser.UserId.Value, deckId, cardId);
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization();

        return group;
    }
}
