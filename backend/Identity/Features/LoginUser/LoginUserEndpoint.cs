using MediatR;

namespace Identity.Features.LoginUser;

public sealed record LoginUserRequest(string Email, string Password);

public static class LoginUserEndpoint
{
    public static RouteGroupBuilder MapLoginUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (
            LoginUserRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Email, request.Password);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Ok(response);
        });

        return group;
    }
}
