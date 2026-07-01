using FluentValidation;
using MediatR;

namespace Study.Features.LogDailyActivity;

public sealed class LogDailyActivityCommandValidator : AbstractValidator<LogDailyActivityCommand>
{
    public LogDailyActivityCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public sealed class LogDailyActivityHandler : IRequestHandler<LogDailyActivityCommand, LogDailyActivityResponse>
{
    public Task<LogDailyActivityResponse> Handle(LogDailyActivityCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
