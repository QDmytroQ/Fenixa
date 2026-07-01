using MediatR;
using Profile.Persistence;
using Shared.IntegrationEvents;

namespace Profile.Integration;

public sealed class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
{
    private readonly ProfileDbContext _dbContext;

    public UserRegisteredEventHandler(ProfileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
