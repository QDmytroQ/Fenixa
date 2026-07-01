using Study.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Study.Persistence.Configurations;

public sealed class UserStatisticsConfiguration : IEntityTypeConfiguration<UserStatistics>
{
    public void Configure(EntityTypeBuilder<UserStatistics> builder)
    {
        builder.ToTable("user_statistics");

        builder.HasKey(s => s.UserId);

        builder.Property(s => s.CurrentStreak)
            .IsRequired();

        builder.Property(s => s.LongestStreak)
            .IsRequired();

        builder.Property(s => s.LearnedCards)
            .IsRequired();

        builder.Property(s => s.ReviewedCards)
            .IsRequired();
    }
}
