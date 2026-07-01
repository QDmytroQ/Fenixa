namespace Content.Domain.Entities;

public class CardTag
{
    public Guid CardId { get; set; }
    public Guid TagId { get; set; }
    public Card Card { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
