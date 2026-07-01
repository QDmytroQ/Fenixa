using MediatR;

namespace Content.Features.UpdateDeck;

public sealed record UpdateDeckCommand(
    Guid UserId,
    Guid DeckId,
    string? Name,
    bool? IsPublic) : IRequest;
