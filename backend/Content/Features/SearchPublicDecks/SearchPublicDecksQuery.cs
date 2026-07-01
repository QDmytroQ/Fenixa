using MediatR;

namespace Content.Features.SearchPublicDecks;

public sealed record SearchPublicDecksQuery(
    string? SearchTerm,
    string? TargetLanguage,
    int Page,
    int PageSize) : IRequest<SearchPublicDecksResponse>;

public sealed record PublicDeckDto(
    Guid Id,
    string Name,
    string TargetLanguage,
    int CardCount);

public sealed record SearchPublicDecksResponse(
    IReadOnlyList<PublicDeckDto> Decks,
    int TotalCount);
