using MediatR;
using Shared.OperationResults;
using Identity.Domain;

namespace Identity.Features.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : IRequest<Result<LoginUserAuthResult>>;


public record LoginUserAuthResult(
    Guid UserId,
    LoginFlowStatus Status,
    string? TwoFactorToken = null,
    string? EmailConfirmationToken = null,
    DateTimeOffset? ExpiresAt = null
);

public sealed record LoginUserResponse(Guid UserId, string NextStep);
