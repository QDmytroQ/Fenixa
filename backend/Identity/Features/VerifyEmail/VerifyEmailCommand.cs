
using Identity.Features.RegisterUser;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Features.VerifyEmail
{
    public sealed record VerifyEmailCommand(Guid UserId, string OtpCode) : IRequest<Result<VerifyEmailResult>>;

    public sealed record VerifyEmailResult(
        Guid UserId,
        string AccessToken,
        DateTimeOffset AccessExpires,
        string RawRefreshToken,
        DateTimeOffset RefreshExpires);
    public sealed record VerifyEmailResponse(Guid UserId);
}

/*
 
 public sealed record RegisterUserCommand(
    string Username,
    string Email,
    string Password) : IRequest<Result<RegisterUserAuthResult>>;

public sealed record RegisterUserAuthResult(Guid UserId, string Token, DateTimeOffset ExpiresAt);
public sealed record RegisterUserResponse(Guid UserId, string Message);*/