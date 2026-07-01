using MediatR;
using Shared.IntegrationEvents;
using Study.Persistence;

namespace Study.Integration;

public sealed class DeckClonedEventHandler : INotificationHandler<DeckClonedEvent>
{
    private readonly StudyDbContext _dbContext;

    public DeckClonedEventHandler(StudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Handle(DeckClonedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
