namespace Content.Domain.Entities;

public class Card
{
    public Guid Id { get; set; }
    public Guid DeckId { get; set; }
    public string FrontText { get; set; } = string.Empty;
    public string BackText { get; set; } = string.Empty;
    public string ContextExample { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public Deck Deck { get; set; } = null!;
    public ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
}
