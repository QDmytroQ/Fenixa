using MediatR;
using Shared.Abstractions;

namespace Content.Features.UpdateDeck;

public sealed record UpdateDeckRequest(string? Name, bool? IsPublic);

public static class UpdateDeckEndpoint
{
    public static RouteGroupBuilder MapUpdateDeckEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/decks/{deckId:guid}", async (
            Guid deckId,
            UpdateDeckRequest request,
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var command = new UpdateDeckCommand(
                currentUser.UserId,
                deckId,
                request.Name,
                request.IsPublic);

            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization();

        return group;
    }
}
