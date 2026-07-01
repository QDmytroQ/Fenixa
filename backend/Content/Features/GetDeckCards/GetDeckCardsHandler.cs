using MediatR;

namespace Content.Features.GetDeckCards;

public sealed class GetDeckCardsHandler : IRequestHandler<GetDeckCardsQuery, GetDeckCardsResponse>
{
    public Task<GetDeckCardsResponse> Handle(GetDeckCardsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
