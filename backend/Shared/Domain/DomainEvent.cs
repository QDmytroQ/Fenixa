using MediatR;

namespace Shared.Domain;

public abstract record DomainEvent : INotification
{
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
}
