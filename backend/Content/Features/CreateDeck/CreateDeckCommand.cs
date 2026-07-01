using MediatR;

namespace Content.Features.CreateDeck;

public sealed record CreateDeckCommand(
    Guid UserId,
    string Name,
    string TargetLanguage) : IRequest<CreateDeckResponse>;

public sealed record CreateDeckResponse(Guid DeckId);
