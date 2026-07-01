using MediatR;

namespace Study.Features.LogDailyActivity;

public sealed record LogDailyActivityCommand(Guid UserId) : IRequest<LogDailyActivityResponse>;

public sealed record LogDailyActivityResponse(int CurrentStreak);
