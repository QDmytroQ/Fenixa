using Study.Domain.Enums;

namespace Study.Domain.Entities;

public class CardProgress
{
    public Guid UserId { get; set; }
    public Guid CardId { get; set; }
    public string TargetLanguage { get; set; } = string.Empty;
    public CardStatus Status { get; set; }
    public DateTimeOffset NextReviewDate { get; set; }
    public DateTimeOffset LastReviewedAt { get; set; }
    public double Stability { get; set; }
    public double Difficulty { get; set; }
    public int Lapses { get; set; }
    public int Reps { get; set; }
}
