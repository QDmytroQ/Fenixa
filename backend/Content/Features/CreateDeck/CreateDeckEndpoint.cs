using MediatR;
using Shared.Abstractions;

namespace Content.Features.CreateDeck;

public sealed record CreateDeckRequest(string Name, string TargetLanguage);

public static class CreateDeckEndpoint
{
    public static RouteGroupBuilder MapCreateDeckEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/decks", async (
            CreateDeckRequest request,
            IMediator mediator,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new CreateDeckCommand(
                currentUser.UserId.Value,
                request.Name,
                request.TargetLanguage);

            var response = await mediator.Send(command, cancellationToken);
            return Results.Created($"/api/content/decks/{response.DeckId}", response);
        })
        .RequireAuthorization();

        return group;
    }
}
