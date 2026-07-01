using MediatR;

namespace Profile.Features.UpdateSettings;

public sealed record UpdateSettingsCommand(
    Guid UserId,
    string? Timezone,
    string? Theme,
    string? AppLanguage,
    string? TargetLanguage,
    string? DailyReminderTime) : IRequest;
