using MediatR;
using Identity.Infrastructure;
using Shared.Web;

namespace Identity.Features.LogOut
{
    public static class LogoutUserEndpoint
    {
        public static RouteGroupBuilder MapLogoutUserEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/logout", async (
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

                var command = new LogoutUserCommand(refreshToken);
                var result = await mediator.Send(command, cancellationToken);

                return result.ToHttpResult( () =>
                {
                    authCookieWriter.Clear();

                    return Results.Ok();
                });
            }).RequireAuthorization();

            return group;
        }
    }
}

