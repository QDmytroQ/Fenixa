using MediatR;

namespace Identity.Features.RegisterUser;

public sealed record RegisterUserCommand(
    string Username,
    string Email,
    string Password) : IRequest<RegisterUserResponse>;

public sealed record RegisterUserResponse(Guid UserId);
