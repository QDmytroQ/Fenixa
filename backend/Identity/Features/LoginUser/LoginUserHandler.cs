using FluentValidation;
using Identity.Domain;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents;
using Shared.Results;

namespace Identity.Features.LoginUser;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public sealed class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<LoginUserAuthResult>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly IOtpService _otpService;
    private readonly TwoFactorTokenGenerator _twoFactorTokenGenerator;
    private readonly EmailVerificationTokenGenerator _emailVerificationTokenGenerator;

    public LoginUserHandler(
        IdentityDbContext dbContext,
        IPublisher publisher,
        IUserPasswordHasher passwordHasher,
        IOtpService otpService,
        EmailVerificationTokenGenerator emailVerificationTokenGenerator,
        TwoFactorTokenGenerator twoFactorTokenGenerator)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _passwordHasher = passwordHasher;
        _otpService = otpService;
        _twoFactorTokenGenerator = twoFactorTokenGenerator;
        _emailVerificationTokenGenerator = emailVerificationTokenGenerator;
    }

    public async Task<Result<LoginUserAuthResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user= await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user == null)
        {
            return Result.Failure<LoginUserAuthResult>(
                Error.Unauthorized("Invalid email or password"));
        }

        var isPasswordValid = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password   
        );

        if (!isPasswordValid)
        {
            return Result.Failure<LoginUserAuthResult>(
                Error.Unauthorized("Invalid email or password"));
        }

        if (!user.EmailConfirmed)
        {
            var otpGenerationResult = await _otpService.GenerateCodeAsync(
                user.Id,
                OtpPurpose.EmailVerification,
                cancellationToken);

            await _publisher.Publish(
                new EmailVerificationRequested(user.Id, user.Username, user.Email, otpGenerationResult.Code),
                cancellationToken);

            var emailConfirmationToken = _emailVerificationTokenGenerator.Generate(user.Id, otpGenerationResult.ExpiresAt);

            return Result.Success(new LoginUserAuthResult(
                UserId: user.Id,
                Status: LoginFlowStatus.RequiresEmailConfirmation,
                EmailConfirmationToken: emailConfirmationToken,
                ExpiresAt: otpGenerationResult.ExpiresAt
            ));
        }
        else
        {
            var otpGenerationResult = await _otpService.GenerateCodeAsync(
                user.Id,
                OtpPurpose.TwoFactorAuth,
                cancellationToken);

            await _publisher.Publish(
                new TwoFactorAuthRequested(user.Id, user.Username, user.Email, otpGenerationResult.Code), cancellationToken);

            var twoFactorToken = _twoFactorTokenGenerator.Generate(user.Id, otpGenerationResult.ExpiresAt);

            return Result.Success(new LoginUserAuthResult(
                UserId: user.Id,
                Status: LoginFlowStatus.RequiresTwoFactor,
                TwoFactorToken: twoFactorToken,
                ExpiresAt: otpGenerationResult.ExpiresAt
            ));
        }
    }
}
