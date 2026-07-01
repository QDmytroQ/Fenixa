using FluentValidation;
using MediatR;

namespace Content.Features.ClonePublicDeck;

public sealed class ClonePublicDeckCommandValidator : AbstractValidator<ClonePublicDeckCommand>
{
    public ClonePublicDeckCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.SourceDeckId).NotEmpty();
    }
}

public sealed class ClonePublicDeckHandler : IRequestHandler<ClonePublicDeckCommand, ClonePublicDeckResponse>
{
    public Task<ClonePublicDeckResponse> Handle(ClonePublicDeckCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
