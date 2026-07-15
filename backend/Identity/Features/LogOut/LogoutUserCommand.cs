using MediatR;
using Shared.OperationResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Features.LogOut
{
    public sealed record LogoutUserCommand(string RefreshToken) : IRequest<Result>;

}
