using MediatR;
using Shared.IntegrationEvents;
using Study.Persistence;

namespace Study.Integration;

public sealed class CardsDeletedEventHandler : INotificationHandler<CardsDeletedEvent>
{
    private readonly StudyDbContext _dbContext;

    public CardsDeletedEventHandler(StudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Handle(CardsDeletedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
