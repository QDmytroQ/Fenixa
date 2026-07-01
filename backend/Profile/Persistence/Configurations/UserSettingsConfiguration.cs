using Profile.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Profile.Persistence.Configurations;

public sealed class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.ToTable("user_settings");

        builder.HasKey(s => s.UserId);

        builder.Property(s => s.Timezone)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(s => s.Theme)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(s => s.AppLanguage)
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(s => s.TargetLanguage)
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(s => s.DailyReminderTime)
            .IsRequired()
            .HasMaxLength(8);
    }
}
