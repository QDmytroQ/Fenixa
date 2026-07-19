using Content.Domain.Entities;
using Content.Features.Shared;
using Content.Infrastructure;
using Content.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using Shared.OperationResults;

namespace Content.Features.AddCard;

public sealed class AddCardCommandValidator : BaseCardCommandValidator<AddCardCommand>
{
    public AddCardCommandValidator()
    {
        RuleFor(x => x.FrontText)
            .NotEmpty()
            .MaximumLength(FrontTextMaxLength); 

        RuleFor(x => x.BackText)
            .NotEmpty()
            .MaximumLength(BackTextMaxLength);
    }
}

public sealed class AddCardHandler : IRequestHandler<AddCardCommand, Result<AddCardResult>>
{
    private readonly ContentDbContext _context;
    private readonly ITagService _tagService;

    public AddCardHandler(ContentDbContext context, ITagService tagService)
    {
        _context = context;
        _tagService = tagService;
    }

    public async Task<Result<AddCardResult>> Handle(AddCardCommand request, CancellationToken cancellationToken)
    {
        var deckExists = await _context.Decks
            .AnyAsync(d => d.Id == request.DeckId && d.UserId == request.UserId, cancellationToken);

        if (!deckExists)
        {
            return Result<AddCardResult>.Failure(Error.NotFound("Deck was not found."));
        }

        var cleanFrontText = request.FrontText.Trim();

        // 1. Попередня перевірка на дублікат тексту
        var isDuplicateText = await _context.Cards.AsNoTracking()
            .AnyAsync(c => c.DeckId == request.DeckId && c.FrontText == cleanFrontText, cancellationToken);

        if (isDuplicateText)
        {
            return Result<AddCardResult>.Failure(Error.Conflict("Card with this Front Text already exists in the deck."));
        }

        var tagsResult = await _tagService.PrepareTagsAsync(request.TagNames, cancellationToken);
        if (tagsResult.IsFailure)
        {
            return Result<AddCardResult>.Failure(tagsResult.Error);
        }

        var card = new Card
        {
            Id = Guid.NewGuid(),
            DeckId = request.DeckId,
            FrontText = cleanFrontText,
            BackText = request.BackText.Trim(),
            ContextExample = request.ContextExample?.Trim() ?? string.Empty,
            AudioUrl = string.Empty
        };

        card.SyncTags(tagsResult.Value);
        _context.Cards.Add(card);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (DbUpdateExceptionExtensions.IsUniqueConstraintViolation(ex))
        {
            var existingCardId = await _context.Cards.AsNoTracking()
                    .Where(c => c.DeckId == request.DeckId && c.FrontText == cleanFrontText)
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync(cancellationToken);
            return Result<AddCardResult>.Success(new AddCardResult(existingCardId));
        }

        return Result<AddCardResult>.Success(new AddCardResult(card.Id));
    }
}
