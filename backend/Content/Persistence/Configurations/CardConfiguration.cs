using Content.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Content.Persistence.Configurations;

public sealed class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("cards");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.DeckId)
            .IsRequired();

        builder.Property(c => c.FrontText)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.BackText)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.ContextExample)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(c => c.AudioUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.HasMany(c => c.CardTags)
            .WithOne(ct => ct.Card)
            .HasForeignKey(ct => ct.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.DeckId);
    }
}
