using MediatR;

namespace Study.Features.GetStudySession;

public sealed class GetStudySessionHandler : IRequestHandler<GetStudySessionQuery, GetStudySessionResponse>
{
    public Task<GetStudySessionResponse> Handle(GetStudySessionQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
