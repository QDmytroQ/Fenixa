using MediatR;
using Shared.IntegrationEvents;

namespace Profile.Features.GetUserSettings;

public sealed record GetUserSettingsQuery(Guid UserId) : IRequest<GetUserSettingsResponse>;

public sealed record GetUserSettingsResponse(
    Guid UserId,
    string? Username,
    string? Email,
    string Timezone,
    string Theme,
    string AppLanguage,
    string TargetLanguage,
    string DailyReminderTime);
