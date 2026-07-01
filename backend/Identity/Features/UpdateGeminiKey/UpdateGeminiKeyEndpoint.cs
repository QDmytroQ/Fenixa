using MediatR;

namespace Identity.Features.UpdateGeminiKey;

public sealed record UpdateGeminiKeyRequest(string ApiKey);

public static class UpdateGeminiKeyEndpoint
{
    public static RouteGroupBuilder MapUpdateGeminiKeyEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/gemini-key", async (
            UpdateGeminiKeyRequest request,
            IMediator mediator,
            Shared.Abstractions.ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new UpdateGeminiKeyCommand(currentUser.UserId.Value, request.ApiKey);
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization();

        return group;
    }
}
