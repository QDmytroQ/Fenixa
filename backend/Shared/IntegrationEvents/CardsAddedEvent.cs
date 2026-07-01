using MediatR;

namespace Shared.IntegrationEvents;

public sealed record CardAddedInfo(Guid CardId, string TargetLanguage);

public sealed record CardsAddedEvent(
    Guid UserId,
    IReadOnlyList<CardAddedInfo> Cards) : INotification;
