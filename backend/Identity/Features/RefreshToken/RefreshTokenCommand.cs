using MediatR;

namespace Identity.Features.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResponse>;

public sealed record RefreshTokenResponse(string AccessToken, string RefreshToken);
