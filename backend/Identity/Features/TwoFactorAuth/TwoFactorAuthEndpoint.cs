using Identity.Features.RegisterUser;
using Identity.Infrastructure;
using MediatR;
using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Features.TwoFactorAuth
{
    public sealed record TwoFactorAuthRequest(string OtpCode);
    public static class TwoFactorAuthEndpoint
    {
        public static RouteGroupBuilder MapTwoFactorAuthEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/two-factor-auth", async (
                TwoFactorAuthRequest request,
                IMediator mediator,
                ITwoFactorAuthCookieWriter twoFactorAuthCookieWriter,
                IAuthCookieWriter authCookieWriter,
                ICurrentUserContext currentUserContext,
                CancellationToken cancellationToken) =>
            {
                var command = new TwoFactorAuthCommand(currentUserContext.UserId, request.OtpCode);
                var result = await mediator.Send(command, cancellationToken);

                return result.ToHttpResult(auth =>
                {

                    twoFactorAuthCookieWriter.Clear();
                    authCookieWriter.Append(
                        auth.AccessToken, 
                        auth.AccessExpires, 
                        auth.RawRefreshToken, 
                        auth.RefreshExpires
                    );
                    return Results.Ok(
                        new TwoFactorAuthResponse(auth.UserId));
                });
            }).RequireAuthorization("TwoFactorAuthOnly");

            return group;
        }
    }
}
/*
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

*/