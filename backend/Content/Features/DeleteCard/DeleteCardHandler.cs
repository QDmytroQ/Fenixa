using FluentValidation;
using MediatR;

namespace Content.Features.DeleteCard;

public sealed class DeleteCardCommandValidator : AbstractValidator<DeleteCardCommand>
{
    public DeleteCardCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DeckId).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
    }
}

public sealed class DeleteCardHandler : IRequestHandler<DeleteCardCommand>
{
    public Task Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
