namespace Study.Domain.Entities;

public class UserStatistics
{
    public Guid UserId { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int LearnedCards { get; set; }
    public int ReviewedCards { get; set; }
}
