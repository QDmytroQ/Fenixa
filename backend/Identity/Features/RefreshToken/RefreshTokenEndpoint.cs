using MediatR;

namespace Identity.Features.RefreshToken;

public sealed record RefreshTokenRequest(string RefreshToken);

public static class RefreshTokenEndpoint
{
    public static RouteGroupBuilder MapRefreshTokenEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/refresh", async (
            RefreshTokenRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization("RefreshPolicy");

        return group;
    }
}
