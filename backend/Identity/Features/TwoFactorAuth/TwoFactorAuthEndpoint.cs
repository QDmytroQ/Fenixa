using Identity.Infrastructure;
using MediatR;
using Shared.Abstractions;
using Shared.Web;
;

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

*/