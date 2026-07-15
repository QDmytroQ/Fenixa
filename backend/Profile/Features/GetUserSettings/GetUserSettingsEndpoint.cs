using MediatR;
using Shared.Abstractions;

namespace Profile.Features.GetUserSettings;

public static class GetUserSettingsEndpoint
{
    public static RouteGroupBuilder MapGetUserSettingsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/settings", async (
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var query = new GetUserSettingsQuery(currentUser.UserId);
            var response = await mediator.Send(query, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
