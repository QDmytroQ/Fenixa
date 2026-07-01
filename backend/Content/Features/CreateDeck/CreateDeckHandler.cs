using FluentValidation;
using MediatR;

namespace Content.Features.CreateDeck;

public sealed class CreateDeckCommandValidator : AbstractValidator<CreateDeckCommand>
{
    public CreateDeckCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TargetLanguage).NotEmpty().MaximumLength(16);
    }
}

public sealed class CreateDeckHandler : IRequestHandler<CreateDeckCommand, CreateDeckResponse>
{
    public Task<CreateDeckResponse> Handle(CreateDeckCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
