using FluentValidation;
using MediatR;

namespace Study.Features.ReviewCard;

public sealed class ReviewCardCommandValidator : AbstractValidator<ReviewCardCommand>
{
    public ReviewCardCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
        RuleFor(x => x.Rating).IsInEnum();
    }
}

public sealed class ReviewCardHandler : IRequestHandler<ReviewCardCommand, ReviewCardResponse>
{
    public Task<ReviewCardResponse> Handle(ReviewCardCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
