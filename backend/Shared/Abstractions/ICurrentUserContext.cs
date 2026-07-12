namespace Shared.Abstractions;

public interface ICurrentUserContext
{
    Guid UserId { get; }
}
