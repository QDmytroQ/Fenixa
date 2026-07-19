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

    public bool UpdateDetails(string? front, string? back, string? context, string? audioUrl)
    {
        var frontChanged = UpdateProperty(front, v => FrontText = v, FrontText);
        var backChanged = UpdateProperty(back, v => BackText = v, BackText);
        var contextChanged = UpdateProperty(context, v => ContextExample = v, ContextExample);
        var audioChanged = UpdateProperty(audioUrl, v => AudioUrl = v, AudioUrl);

        return frontChanged || backChanged || contextChanged || audioChanged;
    }

    public bool SyncTags(IReadOnlyList<Tag> newTags)
    {
        var currentTagIds = CardTags.Select(ct => ct.TagId).ToHashSet();
        var newTagIds = newTags.Select(t => t.Id).ToHashSet();

        if (currentTagIds.SetEquals(newTagIds))
        {
            return false;
        }

        CardTags.Clear();
        foreach (var tag in newTags)
        {
            CardTags.Add(new CardTag { CardId = Id, Card = this, TagId = tag.Id, Tag = tag });
        }

        return true;
    }

    private static bool UpdateProperty(string? newValue, Action<string> setter, string currentValue)
    {
        if (newValue is null)
        {
            return false;
        }

        var trimmed = newValue.Trim();
        if (currentValue == trimmed)
        {
            return false;
        }

        setter(trimmed);
        return true;
    }
}
