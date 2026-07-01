using MediatR;

namespace Identity.Features.RegisterUser;

public sealed record RegisterUserRequest(string Username, string Email, string Password);

public static class RegisterUserEndpoint
{
    public static RouteGroupBuilder MapRegisterUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/register", async (
            RegisterUserRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(request.Username, request.Email, request.Password);
            var response = await mediator.Send(command, cancellationToken);
            return Results.Ok(response);
        });

        return group;
    }
}
