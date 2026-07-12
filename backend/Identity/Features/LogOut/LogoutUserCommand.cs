using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Features.LogOut
{
    public sealed record LogoutUserCommand(string RefreshToken) : IRequest<Result<LogoutUserResult>>;

    public sealed record LogoutUserResult(
        Guid UserId,
        string AccessToken,
        string RawRefreshToken,
        DateTimeOffset RefreshExpires);

    public sealed record LogoutUserResponse(Guid UserId);

}
