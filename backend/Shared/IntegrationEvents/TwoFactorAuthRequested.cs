using MediatR;


namespace Shared.IntegrationEvents
{
    public sealed record TwoFactorAuthRequested(
    Guid UserId,
    string Username,
    string Email,
    string TwoFactorCode) : INotification;
}

