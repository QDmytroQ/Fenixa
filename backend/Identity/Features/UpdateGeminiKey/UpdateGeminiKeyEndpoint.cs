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
            Shared.Abstractions.ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var command = new UpdateGeminiKeyCommand(currentUser.UserId, request.ApiKey);
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization();

        return group;
    }
}
