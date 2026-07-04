using MediatR;
using Shared.Results;

namespace Identity.Features.RegisterUser;

public sealed record RegisterUserCommand(
    string Username,
    string Email,
    string Password) : IRequest<Result<RegisterUserAuthResult>>;

public sealed record RegisterUserAuthResult(
    Guid UserId,
    string AccessToken,
    string RawRefreshToken,
    DateTimeOffset RefreshExpires);

public sealed record RegisterUserResponse(Guid UserId);
