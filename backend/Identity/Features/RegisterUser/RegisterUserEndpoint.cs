using Identity.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Identity.Features.RegisterUser;

public sealed record RegisterUserRequest(string Username, string Email, string Password);

public static class RegisterUserEndpoint
{
    public static RouteGroupBuilder MapRegisterUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/register", async (
            RegisterUserRequest request,
            IMediator mediator,
            IAuthCookieService authCookieService,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(request.Username, request.Email, request.Password);
            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(auth =>
            {
                authCookieService.SetAuthCookies(
                    httpContext,
                    auth.AccessToken,
                    auth.RawRefreshToken,
                    auth.RefreshExpires);

                return Results.Ok(new RegisterUserResponse(auth.UserId));
            });
        });

        return group;
    }
}
