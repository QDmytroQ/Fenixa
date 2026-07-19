namespace Content.Domain.Entities;

public class Deck
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TargetLanguage { get; set; } = string.Empty;
    public bool IsMain { get; set; } = false;
    public bool IsPublic { get; set; }
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}
