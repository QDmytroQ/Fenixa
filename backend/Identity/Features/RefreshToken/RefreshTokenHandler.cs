using FluentValidation;
using MediatR;
using Shared.Results;

namespace Identity.Features.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenAuthResult>>
{
    public Task<Result<RefreshTokenAuthResult>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
