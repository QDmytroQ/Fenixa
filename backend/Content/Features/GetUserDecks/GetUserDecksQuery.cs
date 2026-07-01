using MediatR;

namespace Content.Features.GetUserDecks;

public sealed record GetUserDecksQuery(Guid UserId) : IRequest<GetUserDecksResponse>;

public sealed record DeckSummaryDto(
    Guid Id,
    string Name,
    string TargetLanguage,
    bool IsPublic,
    int CardCount);

public sealed record GetUserDecksResponse(IReadOnlyList<DeckSummaryDto> Decks);
