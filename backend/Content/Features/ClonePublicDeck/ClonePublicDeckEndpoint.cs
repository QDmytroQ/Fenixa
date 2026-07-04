using MediatR;
using Shared.Abstractions;

namespace Content.Features.ClonePublicDeck;

public static class ClonePublicDeckEndpoint
{
    public static RouteGroupBuilder MapClonePublicDeckEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/decks/{sourceDeckId:guid}/clone", async (
            Guid sourceDeckId,
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new ClonePublicDeckCommand(currentUser.UserId.Value, sourceDeckId);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Created($"/api/content/decks/{response.NewDeckId}", response);
        })
        .RequireAuthorization();

        return group;
    }
}
