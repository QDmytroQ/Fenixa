using Content.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Content.Domain.Constants;

namespace Content.Persistence.Configurations;

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(t => t.Id);


        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(TagConstants.TagNameMaxLength);

        builder.HasMany(t => t.CardTags)
            .WithOne(ct => ct.Tag)
            .HasForeignKey(ct => ct.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => t.Name)
            .IsUnique();
    }
}