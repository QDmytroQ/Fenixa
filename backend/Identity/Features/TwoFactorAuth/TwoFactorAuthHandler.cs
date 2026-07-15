using FluentValidation;
using Identity.Domain;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.OperationResults;

namespace Identity.Features.TwoFactorAuth
{
    public sealed class TwoFactorAuthCommandValidator : AbstractValidator<TwoFactorAuthCommand>
    {
        public TwoFactorAuthCommandValidator()
        {
            RuleFor(x => x.OtpCode).NotEmpty().Length(6);
        }
    }
    public sealed class TwoFactorAuthHandler: IRequestHandler<TwoFactorAuthCommand, Result<TwoFactorAuthResult>>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IOtpService _otpService;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public TwoFactorAuthHandler(
            IdentityDbContext dbContext,
            IOtpService otpService,
            IRefreshTokenGenerator refreshTokenGenerator,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _dbContext = dbContext;
            _otpService = otpService;
            _refreshTokenGenerator = refreshTokenGenerator;
            _accessTokenGenerator = accessTokenGenerator;
        }


        public async Task<Result<TwoFactorAuthResult>> Handle(TwoFactorAuthCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                return Result.Failure<TwoFactorAuthResult>(Error.Forbidden("User not found"));
            }

            if (!user.EmailConfirmed)
            {
                return Result.Failure<TwoFactorAuthResult>(Error.Forbidden("Email not confirmed"));
            }

            var isOtpValid = await _otpService.ValidateCodeAsync(user.Id, request.OtpCode, OtpPurpose.TwoFactorAuth, cancellationToken);

            if (!isOtpValid)
            {
                return Result.Failure<TwoFactorAuthResult>(Error.Unauthorized("Invalid OTP code"));
            }

            var refreshTokenPair = _refreshTokenGenerator.Generate();

            var refreshToken = new Domain.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshTokenPair.TokenHash,
                IsValid = true,
                Created = DateTimeOffset.UtcNow,
                Expires = refreshTokenPair.Expires
            };

            _dbContext.RefreshTokens.Add(refreshToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var accessToken = _accessTokenGenerator.Generate(user.Id);

            return Result.Success(
                new TwoFactorAuthResult(user.Id, accessToken.Token, accessToken.Expires, refreshTokenPair.RawToken, refreshTokenPair.Expires)
            );
        }
    }
}