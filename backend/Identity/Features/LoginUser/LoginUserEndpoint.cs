using Identity.Infrastructure;
using Identity.Domain;
using MediatR;
using Shared.Extensions;

namespace Identity.Features.LoginUser;

public sealed record LoginUserRequest(string Email, string Password);

public static class LoginUserEndpoint
{
    public static RouteGroupBuilder MapLoginUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (
            LoginUserRequest request,
            IMediator mediator,
            ITwoFactorAuthCookieWriter twoFactorAuthCookieWriter,
            IEmailVerificationCookieWriter emailConfirmationCookieWriter,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var result = await mediator.Send(command, cancellationToken);

            return result.ToHttpResult(auth =>
            {
                if (auth.Status == LoginFlowStatus.RequiresEmailConfirmation)
                {
                    emailConfirmationCookieWriter.Append(
                        auth.EmailConfirmationToken!,
                        auth.ExpiresAt!.Value);

                    return Results.Ok(new LoginUserResponse(
                        UserId: auth.UserId,
                        NextStep: auth.Status.ToString()
                    ));
                }

                twoFactorAuthCookieWriter.Append(
                    auth.TwoFactorToken!,
                    auth.ExpiresAt!.Value);

                return Results.Ok(new LoginUserResponse(
                    UserId: auth.UserId,
                    NextStep: auth.Status.ToString() // Поверне "RequiresTwoFactor"
                ));
            });
        });

        return group;
    }
}
