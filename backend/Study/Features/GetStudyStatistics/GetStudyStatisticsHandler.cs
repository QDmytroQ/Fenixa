using MediatR;

namespace Study.Features.GetStudyStatistics;

public sealed class GetStudyStatisticsHandler : IRequestHandler<GetStudyStatisticsQuery, GetStudyStatisticsResponse>
{
    public Task<GetStudyStatisticsResponse> Handle(GetStudyStatisticsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
