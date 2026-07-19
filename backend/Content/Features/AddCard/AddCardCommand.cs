using MediatR;
using Shared.OperationResults;
using Content.Features.Shared;

namespace Content.Features.AddCard;

public sealed record AddCardCommand(
    Guid UserId,
    Guid DeckId,
    string FrontText,
    string BackText,
    string ContextExample,
    IReadOnlyList<string>? TagNames) : IRequest<Result<AddCardResult>>, ICardCommand;

public sealed record AddCardResult(Guid CardId);
public sealed record AddCardResponse(Guid CardId);
