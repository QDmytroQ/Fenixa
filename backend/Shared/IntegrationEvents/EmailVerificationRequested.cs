using MediatR;


namespace Shared.IntegrationEvents
{
    public sealed record EmailVerificationRequested(
    Guid UserId,
    string Username,
    string Email,
    string VerificationCode) : INotification;
}
