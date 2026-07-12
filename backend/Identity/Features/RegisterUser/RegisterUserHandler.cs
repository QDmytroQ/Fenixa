using FluentValidation;
using Identity.Domain;
using Identity.Domain.Entities;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents;
using Shared.Results;
using System.Text.RegularExpressions;

namespace Identity.Features.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private const string AllowedPasswordCharacters = "!@#$%";
    private static readonly string AllowedPasswordCharacterSet =
        $"^[A-Za-z0-9{Regex.Escape(AllowedPasswordCharacters)}]+$";

    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(24)
            .Matches(AllowedPasswordCharacterSet)
            .WithMessage($"Password may only contain letters, digits, and symbols {AllowedPasswordCharacters}.")
            .Must(p => p.Any(char.IsUpper))
            .WithMessage("Password must contain at least one uppercase letter.")
            .Must(p => p.Any(char.IsLower))
            .WithMessage("Password must contain at least one lowercase letter.")
            .Must(p => p.Any(char.IsDigit))
            .WithMessage("Password must contain at least one digit.")
            .Must(p => p.Any(c => AllowedPasswordCharacters.Contains(c)))
            .WithMessage($"Password must contain at least one symbol from {AllowedPasswordCharacters}.");
    }
}

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserAuthResult>>
{
    private readonly IdentityDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly IOtpService _otpService;
    private readonly EmailVerificationTokenGenerator _emailVerificationTokenGenerator;
    private readonly IUserPasswordHasher _passwordHasher;


    public RegisterUserHandler(
        IdentityDbContext dbContext,
        IPublisher publisher,
        IOtpService otpService,
        JwtProvider jwtProvider,
        EmailVerificationTokenGenerator emailVerificationTokenGenerator,
        IUserPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _otpService = otpService;
        _emailVerificationTokenGenerator = emailVerificationTokenGenerator;
        _passwordHasher = passwordHasher;

    }

    public async Task<Result<RegisterUserAuthResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailExists = await _dbContext.Users
            .AnyAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (emailExists)
        {
            return Result.Failure<RegisterUserAuthResult>(
                Error.Conflict("This email address is already in use"));
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username.Trim(),
            Email = normalizedEmail,
            GeminiApiKeyEncrypted = string.Empty,
            CreatedAt = DateTimeOffset.UtcNow,
            EmailConfirmed = false,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var otpGenerationResult = await _otpService.GenerateCodeAsync(
            user.Id,
            OtpPurpose.EmailVerification,
            cancellationToken);

        await _publisher.Publish(
            new EmailVerificationRequested(user.Id, user.Username, user.Email, otpGenerationResult.Code),
            cancellationToken);

        var token = _emailVerificationTokenGenerator.Generate(user.Id, otpGenerationResult.ExpiresAt);

        return Result.Success(new RegisterUserAuthResult(user.Id, token, otpGenerationResult.ExpiresAt));
    }
}

/*
 * 
 
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

        var accessToken = _jwtProvider.GenerateAccessToken(user.Id);
*/