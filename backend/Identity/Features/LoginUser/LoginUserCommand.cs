using MediatR;
using Shared.Results;

namespace Identity.Features.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : IRequest<Result<LoginUserAuthResult>>;

public sealed record LoginUserAuthResult(
    Guid UserId,
    string AccessToken,
    string RawRefreshToken,
    DateTimeOffset RefreshExpires);

public sealed record LoginUserResponse(Guid UserId);
