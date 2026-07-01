using MediatR;
using Shared.Abstractions;

namespace Content.Features.GetUserDecks;

public static class GetUserDecksEndpoint
{
    public static RouteGroupBuilder MapGetUserDecksEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/decks", async (
            IMediator mediator,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var query = new GetUserDecksQuery(currentUser.UserId.Value);
            var response = await mediator.Send(query, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
