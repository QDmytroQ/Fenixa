using MediatR;
using Shared.Abstractions;
using Shared.Extensions;

namespace Content.Features.UpdateCard;

public sealed record UpdateCardRequest(
    string? FrontText,
    string? BackText,
    string? ContextExample,
    string? AudioUrl,
    IReadOnlyList<string>? TagNames);

public static class UpdateCardEndpoint
{
    public static RouteGroupBuilder MapUpdateCardEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/decks/{deckId:guid}/cards/{cardId:guid}", async (
            Guid deckId,
            Guid cardId,
            UpdateCardRequest request,
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var command = new UpdateCardCommand(
                currentUser.UserId,
                deckId,
                cardId,
                request.FrontText,
                request.BackText,
                request.ContextExample,
                request.AudioUrl,
                request.TagNames);

            var result = await mediator.Send(command, cancellationToken);
            return result.ToHttpResult(() => Results.NoContent());
        })
        .RequireAuthorization();

        return group;
    }
}
