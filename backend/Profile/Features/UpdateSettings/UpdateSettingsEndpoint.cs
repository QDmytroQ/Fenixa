using MediatR;
using Shared.Abstractions;

namespace Profile.Features.UpdateSettings;

public sealed record UpdateSettingsRequest(
    string? Timezone,
    string? Theme,
    string? AppLanguage,
    string? TargetLanguage,
    string? DailyReminderTime);

public static class UpdateSettingsEndpoint
{
    public static RouteGroupBuilder MapUpdateSettingsEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/settings", async (
            UpdateSettingsRequest request,
            IMediator mediator,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new UpdateSettingsCommand(
                currentUser.UserId.Value,
                request.Timezone,
                request.Theme,
                request.AppLanguage,
                request.TargetLanguage,
                request.DailyReminderTime);

            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization();

        return group;
    }
}
