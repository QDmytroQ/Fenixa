using MediatR;
using Shared.Abstractions;

namespace Study.Features.GetStudyStatistics;

public static class GetStudyStatisticsEndpoint
{
    public static RouteGroupBuilder MapGetStudyStatisticsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/statistics", async (
            IMediator mediator,
            ICurrentUserContext currentUser,
            CancellationToken cancellationToken) =>
        {
            if (currentUser.UserId is null)
            {
                return Results.Unauthorized();
            }

            var query = new GetStudyStatisticsQuery(currentUser.UserId.Value);
            var response = await mediator.Send(query, cancellationToken);
            return Results.Ok(response);
        })
        .RequireAuthorization();

        return group;
    }
}
