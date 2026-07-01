using Study.Domain.Entities;

namespace Study.Infrastructure;

public enum ReviewRating
{
    Again = 1,
    Hard = 2,
    Good = 3,
    Easy = 4
}

public interface IFsrsScheduler
{
    CardProgress ScheduleReview(CardProgress progress, ReviewRating rating, DateTimeOffset reviewedAt);
}
