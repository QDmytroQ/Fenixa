using FluentValidation;
using MediatR;

namespace Content.Features.UpdateCard;

public sealed class UpdateCardCommandValidator : AbstractValidator<UpdateCardCommand>
{
    public UpdateCardCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DeckId).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
        RuleFor(x => x.FrontText).MaximumLength(2000).When(x => x.FrontText is not null);
        RuleFor(x => x.BackText).MaximumLength(2000).When(x => x.BackText is not null);
        RuleFor(x => x.ContextExample).MaximumLength(4000).When(x => x.ContextExample is not null);
        RuleFor(x => x.AudioUrl).MaximumLength(2048).When(x => x.AudioUrl is not null);
    }
}

public sealed class UpdateCardHandler : IRequestHandler<UpdateCardCommand>
{
    public Task Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
