using Content.Domain.Constants;
using Content.Domain.Entities;
using Content.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using Shared.OperationResults;

namespace Content.Infrastructure
{
    public class TagService : ITagService
    {
        private readonly ContentDbContext _context;

        public TagService(ContentDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IReadOnlyList<Tag>>> PrepareTagsAsync(
            IReadOnlyList<string>? rawTagNames,
            CancellationToken cancellationToken)
        {
            if (rawTagNames is null || rawTagNames.Count == 0)
            {
                return Result<IReadOnlyList<Tag>>.Success(Array.Empty<Tag>());
            }

            var normalizedNames = rawTagNames
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (normalizedNames.Count == 0)
            {
                return Result<IReadOnlyList<Tag>>.Success(Array.Empty<Tag>());
            }

            if (normalizedNames.Count > CardConstants.MaxTagsPerCard)
            {
                return Result<IReadOnlyList<Tag>>.Failure(
                    Error.Validation($"A card cannot have more than {CardConstants.MaxTagsPerCard} tags."));
            }

            var existingTags = await _context.Tags
                .Where(t => normalizedNames.Contains(t.Name))
                .ToListAsync(cancellationToken);

            var existingNames = existingTags.Select(t => t.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var newNames = normalizedNames.Where(name => !existingNames.Contains(name)).ToList();

            if (newNames.Count > 0)
            {
                var newTags = newNames.Select(name => new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = name
                }).ToList();

                _context.Tags.AddRange(newTags);

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    existingTags.AddRange(newTags);
                }
                catch (DbUpdateException ex) when (DbUpdateExceptionExtensions.IsUniqueConstraintViolation(ex))
                {
                    // 1. Unattach the entities that could not be inserted so they don't cause subsequent SaveChanges to fail
                    foreach (var entry in _context.ChangeTracker.Entries<Tag>().Where(e => e.State == EntityState.Added))
                    {
                        entry.State = EntityState.Detached;
                    }

                    existingTags = await _context.Tags
                        .Where(t => normalizedNames.Contains(t.Name))
                        .ToListAsync(cancellationToken);
                }
            }

            return Result<IReadOnlyList<Tag>>.Success(existingTags);
        }
    }
}
