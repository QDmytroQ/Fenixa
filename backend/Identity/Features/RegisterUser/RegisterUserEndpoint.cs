using Identity.Infrastructure;
using MediatR;
using Shared.Extensions;

namespace Identity.Features.RegisterUser;

public sealed record RegisterUserRequest(string Username, string Email, string Password);

public static class RegisterUserEndpoint
{
    public static RouteGroupBuilder MapRegisterUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/register", async (
            RegisterUserRequest request,
            IMediator mediator,
            IEmailVerificationCookieWriter emailVerificationCookieWriter,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(request.Username, request.Email, request.Password);
            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(auth =>
            {
                emailVerificationCookieWriter.Append(auth.Token, auth.ExpiresAt);
                return Results.Ok(
                    new RegisterUserResponse(auth.UserId, $"To complete your registration, enter the confirmation code sent to your email address. The code will expire at {auth.ExpiresAt}."));
            });
        });

        return group;
    }
}
