using FluentValidation;
using Identity.Domain.Entities;
using Identity.Features.RegisterUser;
using Identity.Infrastructure;
using Identity.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    private readonly JwtProvider _jwtProvider;
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public LoginUserHandler(
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

    public async Task<Result<LoginUserAuthResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user= await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

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

        var accessToken = _jwtProvider.GenerateAccessToken(user.Id);

        return Result.Success(new LoginUserAuthResult(
            user.Id,
            accessToken,
            refreshTokenPair.RawToken,
            refreshTokenPair.Expires));
    }
}
