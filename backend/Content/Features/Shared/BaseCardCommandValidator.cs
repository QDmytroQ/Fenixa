using FluentValidation;
using Content.Features.Shared;


namespace Content.Features.Shared
{
    public abstract class BaseCardCommandValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : ICardCommand
    {
        public const int FrontTextMaxLength = 120;
        public const int BackTextMaxLength = 120;
        public const int ContextExampleMaxLength = 200;
        public const int TagNameMaxLength = 30;
        public const int MaxTagsPerCard = 5;

        protected BaseCardCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.DeckId).NotEmpty();

            RuleFor(x => x.ContextExample)
                .MaximumLength(ContextExampleMaxLength);

            RuleFor(x => x.TagNames)
                .Must(tags => tags == null || tags.Count <= MaxTagsPerCard)
                .WithMessage("A card cannot have more than 5 tags.");

            RuleForEach(x => x.TagNames)
                .NotEmpty()
                .MaximumLength(TagNameMaxLength);

            RuleFor(x => x.TagNames)
                .Must(tags =>
                    tags == null ||
                    tags.Select(t => t.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .Count() == tags.Count)
                .WithMessage("Tag names must be unique within the request.");
        }
    }
}
