using MediatR;
using Shared.IntegrationEvents;
using Study.Persistence;

namespace Study.Integration;

public sealed class CardsAddedEventHandler : INotificationHandler<CardsAddedEvent>
{
    private readonly StudyDbContext _dbContext;

    public CardsAddedEventHandler(StudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Handle(CardsAddedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
