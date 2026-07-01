using FluentValidation;
using MediatR;

namespace Content.Features.DeleteDeck;

public sealed class DeleteDeckCommandValidator : AbstractValidator<DeleteDeckCommand>
{
    public DeleteDeckCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DeckId).NotEmpty();
    }
}

public sealed class DeleteDeckHandler : IRequestHandler<DeleteDeckCommand>
{
    public Task Handle(DeleteDeckCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
