using FluentValidation;
using Identity.Domain.Entities;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents;
using Shared.Results;
using System.Collections.ObjectModel;
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
    private readonly JwtProvider _jwtProvider;
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RegisterUserHandler(
        IdentityDbContext dbContext,
        IPublisher publisher,
        JwtProvider jwtProvider,
        IUserPasswordHasher passwordHasher,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _refreshTokenGenerator = refreshTokenGenerator;
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
            CreatedAt = DateTimeOffset.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

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

        _dbContext.Users.Add(user);
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(
            new UserRegisteredEvent(user.Id, user.Username, user.Email),
            cancellationToken);

        var accessToken = _jwtProvider.GenerateAccessToken(user.Id);

        return Result.Success(new RegisterUserAuthResult(
            user.Id,
            accessToken,
            refreshTokenPair.RawToken,
            refreshTokenPair.Expires));
    }
}
