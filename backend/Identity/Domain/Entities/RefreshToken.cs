namespace Identity.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Token { get; init; } = string.Empty;
    public bool IsValid { get; set; }
    public DateTimeOffset Created { get; init; }
    public DateTimeOffset Expires { get; init; }
    public DateTimeOffset? RevokedAt { get; set; }
    public User User { get; set; } = null!;
}
