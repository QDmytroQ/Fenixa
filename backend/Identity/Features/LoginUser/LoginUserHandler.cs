using FluentValidation;
using MediatR;
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
    public Task<Result<LoginUserAuthResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
