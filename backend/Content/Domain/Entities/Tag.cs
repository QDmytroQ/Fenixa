namespace Content.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
}
