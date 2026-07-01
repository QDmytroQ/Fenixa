using Study.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Study.Persistence.Configurations;

public sealed class CardProgressConfiguration : IEntityTypeConfiguration<CardProgress>
{
    public void Configure(EntityTypeBuilder<CardProgress> builder)
    {
        builder.ToTable("card_progress");

        builder.HasKey(cp => new { cp.UserId, cp.CardId });

        builder.Property(cp => cp.TargetLanguage)
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(cp => cp.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(cp => cp.NextReviewDate)
            .IsRequired();

        builder.Property(cp => cp.LastReviewedAt)
            .IsRequired();

        builder.Property(cp => cp.Stability)
            .IsRequired();

        builder.Property(cp => cp.Difficulty)
            .IsRequired();

        builder.Property(cp => cp.Lapses)
            .IsRequired();

        builder.Property(cp => cp.Reps)
            .IsRequired();

        builder.HasIndex(cp => new { cp.UserId, cp.NextReviewDate });
        builder.HasIndex(cp => new { cp.UserId, cp.TargetLanguage });
    }
}
