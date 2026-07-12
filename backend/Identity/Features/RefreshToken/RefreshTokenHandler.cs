using Azure.Core;
using FluentValidation;
using Identity.Domain.Entities;
using Identity.Features.LoginUser;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Shared.Results;

namespace Identity.Features.RefreshToken;

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenAuthResult>>
{
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IdentityDbContext _dbContext;
    private readonly ICurrentUserContext _currentUser;
    private readonly JwtProvider _jwtProvider;

    public RefreshTokenHandler(IRefreshTokenGenerator refreshTokenGenerator, JwtProvider jwtProvider, IdentityDbContext dbContext, ICurrentUserContext currentUser)
    {
        _refreshTokenGenerator = refreshTokenGenerator;
        _jwtProvider = jwtProvider;
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<Result<RefreshTokenAuthResult>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var tokenHash = _refreshTokenGenerator.HashToken(request.RefreshToken);

        var currentToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == tokenHash && t.Expires > DateTimeOffset.UtcNow, cancellationToken);

        if (currentToken == null)
        {
            return Result.Failure<RefreshTokenAuthResult>(
                Error.Unauthorized("Invalid or expired refresh token"));
        }

        if (!currentToken.IsValid)
        {
            if (currentToken.RevokedAt!.Value.AddSeconds(10) < DateTimeOffset.UtcNow)
            {
                await _dbContext.RefreshTokens
                    .Where(t => t.UserId == currentToken.UserId && t.IsValid)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(t => t.IsValid, false)
                        .SetProperty(t => t.RevokedAt, DateTimeOffset.UtcNow),
                        cancellationToken);

                return Result.Failure<RefreshTokenAuthResult>(
                    Error.Unauthorized("Refresh token reuse detected. All sessions revoked."));
            }
        }
        else
        {
            currentToken.IsValid = false;
            currentToken.RevokedAt = DateTimeOffset.UtcNow;
        }

        currentToken.IsValid = false;
        currentToken.RevokedAt = DateTimeOffset.UtcNow;

        var newRefreshTokenPair = _refreshTokenGenerator.Generate();

        var expirationDate = currentToken.Expires;

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = currentToken.UserId,
            Token = newRefreshTokenPair.TokenHash,
            IsValid = true,
            Created = DateTimeOffset.UtcNow,
            Expires = expirationDate
        };

        _dbContext.RefreshTokens.Add(newRefreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var accessToken = _jwtProvider.GenerateAccessToken(currentToken.UserId);

        return Result.Success(new RefreshTokenAuthResult(
            currentToken.UserId,
            accessToken, 
            newRefreshTokenPair.RawToken,
            expirationDate));
    }
}
