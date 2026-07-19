using Azure;
using Content.Domain.Entities;
using MediatR;
using Shared.Abstractions;
using Shared.OperationResults;
using Shared.Extensions;

namespace Content.Features.AddCard;

public sealed record AddCardRequest(
    Guid? DeckId,
    string FrontText,
    string BackText,
    string ContextExample,
    IReadOnlyList<string> TagNames);

public static class AddCardEndpoint
{
    public static RouteGroupBuilder MapAddCardEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/decks/{deckId:guid}/cards", async (
            Guid deckId,
            AddCardRequest request,
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var command = new AddCardCommand(
                currentUser.UserId,
                deckId,
                request.FrontText,
                request.BackText,
                request.ContextExample,
                request.TagNames);

            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(auth =>
            {

                return Results.Created($"/api/content/decks/{deckId}/cards/{result.Value.CardId}", result);
            });

        }).RequireAuthorization();

        return group;
    }
}
