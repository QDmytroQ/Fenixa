using MediatR;

namespace Study.Features.GetStudyStatistics;

public sealed record GetStudyStatisticsQuery(Guid UserId) : IRequest<GetStudyStatisticsResponse>;

public sealed record GetStudyStatisticsResponse(
    int LearnedCards,
    int ReviewedCards,
    int CurrentStreak,
    int LongestStreak);
