using Identity.Infrastructure;
using MediatR;
using Shared.Abstractions;
using Shared.Web;

namespace Identity.Features.VerifyEmail
{
    public sealed record VerifyEmailRequest(string OtpCode);
    public static class VerifyEmailEndpoint
    {
        public static RouteGroupBuilder MapVerifyEmailEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/verify-email", async (
                VerifyEmailRequest verifyEmailRequest,
                IMediator mediator,
                IAuthCookieWriter authCookieWriter,
                IEmailVerificationCookieWriter emailVerificationCookieWriter,
                HttpContext httpContext,
                ICurrentUserContext currentUser,
                CancellationToken cancellationToken) =>
            {
                var command = new VerifyEmailCommand(currentUser.UserId, verifyEmailRequest.OtpCode);
                var result = await mediator.Send(command, cancellationToken);

                return result.ToHttpResult(auth =>
                {
                    emailVerificationCookieWriter.Clear();

                    authCookieWriter.Append(
                        auth.AccessToken,
                        auth.AccessExpires,
                        auth.RawRefreshToken,
                        auth.RefreshExpires
                        );

                    return Results.Ok(new VerifyEmailResponse(auth.UserId));
                });
            }).RequireAuthorization("EmailVerificationOnly");

            return group;
        }
    }
}
