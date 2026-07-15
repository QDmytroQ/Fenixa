using Identity.Features.RefreshToken;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Shared.OperationResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Features.LogOut
{
    public sealed class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Result>
    {
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IdentityDbContext _dbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly JwtProvider _jwtProvider;

        public LogoutUserHandler(IRefreshTokenGenerator refreshTokenGenerator, JwtProvider jwtProvider, IdentityDbContext dbContext, ICurrentUserContext currentUser)
        {
            _refreshTokenGenerator = refreshTokenGenerator;
            _jwtProvider = jwtProvider;
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(
            LogoutUserCommand request,
            CancellationToken cancellationToken)
        {
            var tokenHash = _refreshTokenGenerator.HashToken(request.RefreshToken);

            var currentToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == tokenHash, cancellationToken);

            if (currentToken != null && currentToken.IsValid)
            {
                currentToken.IsValid = false;
                currentToken.RevokedAt = DateTimeOffset.UtcNow;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
       
            return Result.Success();
        }
    }
}
