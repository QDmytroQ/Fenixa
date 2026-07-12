namespace Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string GeminiApiKeyEncrypted { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public bool EmailConfirmed { get; set; } = false;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
