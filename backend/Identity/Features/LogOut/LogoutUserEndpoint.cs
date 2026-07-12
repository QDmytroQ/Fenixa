using Identity.Features.LogOut;
using MediatR;
using Identity.Infrastructure;

namespace Identity.Features.LogOut
{
    public static class LogoutUserEndpoint
    {
        public static RouteGroupBuilder MapLogoutUserEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/logout", async (
                IMediator mediator,
                IAuthCookieWriter authCookieService,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var refreshToken = httpContext.Request.Cookies[AuthCookieWriter.RefreshTokenCookieName];
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Results.Unauthorized();
                }

                var command = new LogoutUserCommand(refreshToken);
                var result = await mediator.Send(command, cancellationToken);

                return result.ToHttpResult(auth =>
                {
                    authCookieService.Clear();

                    return Results.Ok(new LogoutUserResponse(auth.UserId));
                });
            });

            return group;
        }
    }
}

