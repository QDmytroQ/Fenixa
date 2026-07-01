using FluentValidation;
using MediatR;

namespace Content.Features.AddCard;

public sealed class AddCardCommandValidator : AbstractValidator<AddCardCommand>
{
    public AddCardCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DeckId).NotEmpty();
        RuleFor(x => x.FrontText).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.BackText).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.ContextExample).MaximumLength(4000);
        RuleFor(x => x.AudioUrl).MaximumLength(2048);
    }
}

public sealed class AddCardHandler : IRequestHandler<AddCardCommand, AddCardResponse>
{
    public Task<AddCardResponse> Handle(AddCardCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
