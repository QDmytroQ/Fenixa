using Content.Features.Shared;
using Content.Infrastructure;
using Content.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.OperationResults;
using Shared.Extensions;

namespace Content.Features.UpdateCard;

public sealed class UpdateCardCommandValidator : BaseCardCommandValidator<UpdateCardCommand>
{
    public UpdateCardCommandValidator()
    {
        RuleFor(x => x.CardId).NotEmpty();

        RuleFor(x => x.FrontText)
            .NotEmpty()
            .MaximumLength(FrontTextMaxLength)
            .When(x => x.FrontText is not null);

        RuleFor(x => x.BackText)
            .NotEmpty()
            .MaximumLength(BackTextMaxLength)
            .When(x => x.BackText is not null);
    }
}

public sealed class UpdateCardHandler : IRequestHandler<UpdateCardCommand, Result>
{
    private readonly ContentDbContext _context;
    private readonly ITagService _tagService;

    public UpdateCardHandler(ContentDbContext context, ITagService tagService)
    {
        _context = context;
        _tagService = tagService;
    }

    public async Task<Result> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var isDuplicateText = await _context.Cards.AsNoTracking()
            .AnyAsync(c => c.DeckId == request.DeckId
                        && c.Id != request.CardId
                        && c.FrontText == request.FrontText,
                cancellationToken);

        if (isDuplicateText)
        {
            return Result.Failure(Error.Conflict("Card with this Front Text already exists in the deck."));
        }


        var card = await _context.Cards
            .Include(c => c.CardTags)
            .FirstOrDefaultAsync(
                c => c.Id == request.CardId && c.DeckId == request.DeckId && c.Deck.UserId == request.UserId,
                cancellationToken);

        if (card is null)
        {
            return Result.Failure(Error.NotFound("Card was not found."));
        }

        var detailsChanged = card.UpdateDetails(
            request.FrontText,
            request.BackText,
            request.ContextExample,
            request.AudioUrl);

        var tagsChanged = false;

        if (request.TagNames is not null)
        {
            var tagsResult = await _tagService.PrepareTagsAsync(request.TagNames, cancellationToken);
            if (tagsResult.IsFailure)
            {
                return Result.Failure(tagsResult.Error);
            }

            tagsChanged = card.SyncTags(tagsResult.Value);
        }

        if (!detailsChanged && !tagsChanged)
        {
            return Result.Success();
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (DbUpdateExceptionExtensions.IsUniqueConstraintViolation(ex))
        {
            return Result.Success();
        }

        return Result.Success();
    }
}
