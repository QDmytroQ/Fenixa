using MediatR;

namespace Content.Features.DeleteDeck;

public sealed record DeleteDeckCommand(Guid UserId, Guid DeckId) : IRequest;
