using MediatR;
using Shared.Abstractions;

namespace Study.Features.LogDailyActivity;

public static class LogDailyActivityEndpoint
{
    public static RouteGroupBuilder MapLogDailyActivityEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/activity/daily", async (
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var command = new LogDailyActivityCommand(currentUser.UserId);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
