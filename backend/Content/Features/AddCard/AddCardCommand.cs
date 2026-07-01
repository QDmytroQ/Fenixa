using MediatR;

namespace Content.Features.AddCard;

public sealed record AddCardCommand(
    Guid UserId,
    Guid DeckId,
    string FrontText,
    string BackText,
    string ContextExample,
    string AudioUrl,
    IReadOnlyList<string> TagNames) : IRequest<AddCardResponse>;

public sealed record AddCardResponse(Guid CardId);
