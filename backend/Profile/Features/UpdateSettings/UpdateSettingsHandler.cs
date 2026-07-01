using FluentValidation;
using MediatR;

namespace Profile.Features.UpdateSettings;

public sealed class UpdateSettingsCommandValidator : AbstractValidator<UpdateSettingsCommand>
{
    public UpdateSettingsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public sealed class UpdateSettingsHandler : IRequestHandler<UpdateSettingsCommand>
{
    public Task Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
