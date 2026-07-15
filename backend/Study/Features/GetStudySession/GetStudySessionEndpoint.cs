using MediatR;
using Shared.Abstractions;

namespace Study.Features.GetStudySession;

public static class GetStudySessionEndpoint
{
    public static RouteGroupBuilder MapGetStudySessionEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/session", async (
            string? targetLanguage,
            int maxCards,
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var query = new GetStudySessionQuery(
                currentUser.UserId,
                targetLanguage,
                maxCards <= 0 ? 20 : maxCards);

            var response = await mediator.Send(query, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
