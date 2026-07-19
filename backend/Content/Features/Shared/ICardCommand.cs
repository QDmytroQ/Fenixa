using System;
using System.Collections.Generic;
using System.Text;

namespace Content.Features.Shared
{
    public interface ICardCommand
    {
        Guid UserId { get; }
        Guid DeckId { get; }
        string? ContextExample { get; }
        IReadOnlyList<string>? TagNames { get; }
    }
}
