using MediatR;
using Shared.Results;

namespace Identity.Features.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<RefreshTokenAuthResult>>;

public sealed record RefreshTokenAuthResult(
    Guid UserId,
    string AccessToken,
    string RawRefreshToken,
    DateTimeOffset RefreshExpires);

public sealed record RefreshTokenResponse(Guid UserId);
