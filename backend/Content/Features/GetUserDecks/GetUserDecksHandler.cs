using MediatR;

namespace Content.Features.GetUserDecks;

public sealed class GetUserDecksHandler : IRequestHandler<GetUserDecksQuery, GetUserDecksResponse>
{
    public Task<GetUserDecksResponse> Handle(GetUserDecksQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
