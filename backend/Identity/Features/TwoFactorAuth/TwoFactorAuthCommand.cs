using Identity.Features.VerifyEmail;
using MediatR;
using Shared.OperationResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Features.TwoFactorAuth
{
    public sealed record TwoFactorAuthCommand(Guid UserId, string OtpCode) : IRequest<Result<TwoFactorAuthResult>>;

    public sealed record TwoFactorAuthResult(
        Guid UserId,
        string AccessToken,
        DateTimeOffset AccessExpires,
        string RawRefreshToken,
        DateTimeOffset RefreshExpires);
    public sealed record TwoFactorAuthResponse(Guid UserId);

}

/*
    public sealed record VerifyEmailCommand(Guid UserId, string OtpCode) : IRequest<Result<VerifyEmailResult>>;

    public sealed record VerifyEmailResult(
        Guid UserId,
        string AccessToken,
        string RawRefreshToken,
        DateTimeOffset RefreshExpires);
    public sealed record VerifyEmailResponse(Guid UserId);
*/