using Identity.Infrastructure;
using MediatR;
using Shared.Extensions;

namespace Identity.Features.RefreshToken;

public static class RefreshTokenEndpoint
{
    public static RouteGroupBuilder MapRefreshTokenEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/refresh", async (
            IMediator mediator,
            IAuthCookieWriter authCookieWriter,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var refreshToken = httpContext.Request.Cookies[IdentityCookieWriter.RefreshTokenCookieName];
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Results.Unauthorized();
            }

            var command = new RefreshTokenCommand(refreshToken);
            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(auth =>
            {
                authCookieWriter.Append(
                    auth.AccessToken,
                    auth.AccessExpires,
                    auth.RawRefreshToken,
                    auth.RefreshExpires);

                return Results.Ok(new RefreshTokenResponse(auth.UserId));
            });
        }).RequireAuthorization();

        return group;
    }
}
