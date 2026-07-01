using MediatR;

namespace Shared.IntegrationEvents;

public sealed record UserRegisteredEvent(
    Guid UserId,
    string Username,
    string Email) : INotification;
