using MediatR;

namespace Content.Features.DeleteCard;

public sealed record DeleteCardCommand(
    Guid UserId,
    Guid DeckId,
    Guid CardId) : IRequest;
