namespace Profile.Domain.Entities;

public class UserSettings
{
    public Guid UserId { get; set; }
    public string Timezone { get; set; } = "UTC";
    public string Theme { get; set; } = "system";
    public string AppLanguage { get; set; } = "en";
    public string TargetLanguage { get; set; } = string.Empty;
    public string DailyReminderTime { get; set; } = "09:00";
}
