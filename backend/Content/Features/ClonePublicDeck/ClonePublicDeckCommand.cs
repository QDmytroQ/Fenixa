using MediatR;

namespace Content.Features.ClonePublicDeck;

public sealed record ClonePublicDeckCommand(
    Guid UserId,
    Guid SourceDeckId) : IRequest<ClonePublicDeckResponse>;

public sealed record ClonePublicDeckResponse(Guid NewDeckId);
