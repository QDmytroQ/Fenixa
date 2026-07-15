using MediatR;
using Shared.OperationResults;

namespace Identity.Features.RegisterUser;

public sealed record RegisterUserCommand(
    string Username,
    string Email,
    string Password) : IRequest<Result<RegisterUserAuthResult>>;

public sealed record RegisterUserAuthResult(Guid UserId, string Token, DateTimeOffset ExpiresAt);
public sealed record RegisterUserResponse(Guid UserId, string Message);

/*
    string AccessToken,
    string RawRefreshToken,
    DateTimeOffset RefreshExpires
 */