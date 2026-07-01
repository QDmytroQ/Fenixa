using FluentValidation;
using MediatR;

namespace Identity.Features.UpdateGeminiKey;

public sealed class UpdateGeminiKeyCommandValidator : AbstractValidator<UpdateGeminiKeyCommand>
{
    public UpdateGeminiKeyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ApiKey).NotEmpty();
    }
}

public sealed class UpdateGeminiKeyHandler : IRequestHandler<UpdateGeminiKeyCommand>
{
    public Task Handle(UpdateGeminiKeyCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
