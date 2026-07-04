using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Identity.Features.LoginUser;

public sealed record LoginUserRequest(string Email, string Password);

public static class LoginUserEndpoint
{
    public static RouteGroupBuilder MapLoginUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (
            LoginUserRequest request,
            IMediator mediator,
            IAuthCookieService authCookieService,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(auth =>
            {
                authCookieService.SetAuthCookies(
                    httpContext,
                    auth.AccessToken,
                    auth.RawRefreshToken,
                    auth.RefreshExpires);

                return Results.Ok(new LoginUserResponse(auth.UserId));
            });
        });

        return group;
    }
}
