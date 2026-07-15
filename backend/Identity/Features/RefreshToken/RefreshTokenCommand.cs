using MediatR;
using Shared.OperationResults;

namespace Identity.Features.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<Result<RefreshTokenAuthResult>>;

public sealed record RefreshTokenAuthResult(
    Guid UserId,
    string AccessToken,
    DateTimeOffset AccessExpires,
    string RawRefreshToken,
    DateTimeOffset RefreshExpires);

public sealed record RefreshTokenResponse(Guid UserId);
