using MediatR;
using Shared.Abstractions;

namespace Content.Features.AddCard;

public sealed record AddCardRequest(
    string FrontText,
    string BackText,
    string ContextExample,
    string AudioUrl,
    IReadOnlyList<string> TagNames);

public static class AddCardEndpoint
{
    public static RouteGroupBuilder MapAddCardEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/decks/{deckId:guid}/cards", async (
            Guid deckId,
            AddCardRequest request,
            IMediator mediator,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new AddCardCommand(
                currentUser.UserId.Value,
                deckId,
                request.FrontText,
                request.BackText,
                request.ContextExample,
                request.AudioUrl,
                request.TagNames);

            var response = await mediator.Send(command, cancellationToken);
            return Results.Created($"/api/content/decks/{deckId}/cards/{response.CardId}", response);
        })
        .RequireAuthorization();

        return group;
    }
}
