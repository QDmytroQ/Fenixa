using FluentValidation;
using Identity.Domain;
using Identity.Domain.Entities;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents;
using Shared.OperationResults;


namespace Identity.Features.VerifyEmail
{
    public sealed class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.OtpCode).NotEmpty().Length(6);
        }
    }

    public sealed class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, Result<VerifyEmailResult>>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPublisher _publisher;
        private readonly IOtpService _otpService;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        public VerifyEmailHandler(
            IdentityDbContext dbContext,
            IPublisher publisher,
            IOtpService otpService,
            IRefreshTokenGenerator refreshTokenGenerator,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _dbContext = dbContext;
            _publisher = publisher;
            _otpService = otpService;
            _refreshTokenGenerator = refreshTokenGenerator;
            _accessTokenGenerator = accessTokenGenerator;
        }
        public async Task<Result<VerifyEmailResult>> Handle(
            VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                return Result.Failure<VerifyEmailResult>(
                    Error.Forbidden("User not found"));
            }

            var isOtpValid = await _otpService.ValidateCodeAsync(user.Id, request.OtpCode, OtpPurpose.EmailVerification, cancellationToken);

            if (!isOtpValid)
            {
                return Result.Failure<VerifyEmailResult>(
                    Error.Unauthorized("Invalid OTP code"));
            }

            user.EmailConfirmed = true;

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

            await _publisher.Publish( new UserRegisteredEvent(user.Id, user.Username, user.Email), cancellationToken);

            var accessToken = _accessTokenGenerator.Generate(user.Id);

            return Result.Success(
                new VerifyEmailResult(
                    user.Id, accessToken.Token, accessToken.Expires, refreshTokenPair.RawToken, refreshTokenPair.Expires
                )
            );
        }
    }

}
