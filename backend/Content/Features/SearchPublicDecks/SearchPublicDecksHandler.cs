using MediatR;

namespace Content.Features.SearchPublicDecks;

public sealed class SearchPublicDecksHandler : IRequestHandler<SearchPublicDecksQuery, SearchPublicDecksResponse>
{
    public Task<SearchPublicDecksResponse> Handle(SearchPublicDecksQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
