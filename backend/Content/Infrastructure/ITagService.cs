using Content.Domain.Entities;
using Shared.OperationResults;

namespace Content.Infrastructure
{
    public interface ITagService
    {
        Task<Result<IReadOnlyList<Tag>>> PrepareTagsAsync(IReadOnlyList<string>? rawTagNames, CancellationToken cancellationToken);
    }
}
