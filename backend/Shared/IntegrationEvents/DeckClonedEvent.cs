using MediatR;

namespace Shared.IntegrationEvents;

public sealed record CardIdMapping(Guid SourceCardId, Guid NewCardId, string TargetLanguage);

public sealed record DeckClonedEvent(
    Guid UserId,
    IReadOnlyList<CardIdMapping> CardMappings) : INotification;
