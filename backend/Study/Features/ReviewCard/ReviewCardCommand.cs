using MediatR;
using Study.Infrastructure;

namespace Study.Features.ReviewCard;

public sealed record ReviewCardCommand(
    Guid UserId,
    Guid CardId,
    ReviewRating Rating) : IRequest<ReviewCardResponse>;

public sealed record ReviewCardResponse(
    DateTimeOffset NextReviewDate,
    string Status);
