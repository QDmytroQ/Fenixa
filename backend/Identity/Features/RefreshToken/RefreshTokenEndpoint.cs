using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Identity.Features.RefreshToken;

public static class RefreshTokenEndpoint
{
    public static RouteGroupBuilder MapRefreshTokenEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/refresh", async (
            IMediator mediator,
            IAuthCookieService authCookieService,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var refreshToken = httpContext.Request.Cookies[AuthCookieService.RefreshTokenCookieName];
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Results.Unauthorized();
            }

            var command = new RefreshTokenCommand(refreshToken);
            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(auth =>
            {
                authCookieService.SetAuthCookies(
                    httpContext,
                    auth.AccessToken,
                    auth.RawRefreshToken,
                    auth.RefreshExpires);

                return Results.Ok(new RefreshTokenResponse(auth.UserId));
            });
        });

        return group;
    }
}
