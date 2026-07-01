using FluentValidation;
using MediatR;

namespace Content.Features.UpdateDeck;

public sealed class UpdateDeckCommandValidator : AbstractValidator<UpdateDeckCommand>
{
    public UpdateDeckCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DeckId).NotEmpty();
        RuleFor(x => x.Name).MaximumLength(200).When(x => x.Name is not null);
    }
}

public sealed class UpdateDeckHandler : IRequestHandler<UpdateDeckCommand>
{
    public Task Handle(UpdateDeckCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
