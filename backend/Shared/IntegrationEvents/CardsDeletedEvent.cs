using MediatR;

namespace Shared.IntegrationEvents;

public sealed record CardsDeletedEvent(
    Guid UserId,
    IReadOnlyList<Guid> CardIds) : INotification;
