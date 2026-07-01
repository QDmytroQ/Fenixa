using MediatR;
using Shared.Abstractions;
using Study.Infrastructure;

namespace Study.Features.ReviewCard;

public sealed record ReviewCardRequest(ReviewRating Rating);

public static class ReviewCardEndpoint
{
    public static RouteGroupBuilder MapReviewCardEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/cards/{cardId:guid}/review", async (
            Guid cardId,
            ReviewCardRequest request,
            IMediator mediator,
            ICurrentUserService currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var command = new ReviewCardCommand(currentUser.UserId.Value, cardId, request.Rating);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
