using MediatR;

namespace Content.Features.UpdateCard;

public sealed record UpdateCardCommand(
    Guid UserId,
    Guid DeckId,
    Guid CardId,
    string? FrontText,
    string? BackText,
    string? ContextExample,
    string? AudioUrl) : IRequest;
