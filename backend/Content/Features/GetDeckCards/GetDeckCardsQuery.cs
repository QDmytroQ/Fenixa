using MediatR;

namespace Content.Features.GetDeckCards;

public sealed record GetDeckCardsQuery(Guid UserId, Guid DeckId) : IRequest<GetDeckCardsResponse>;

public sealed record CardDto(
    Guid Id,
    string FrontText,
    string BackText,
    string ContextExample,
    string AudioUrl,
    IReadOnlyList<string> Tags);

public sealed record GetDeckCardsResponse(
    Guid DeckId,
    string DeckName,
    IReadOnlyList<CardDto> Cards);
