using Profile.Domain.Entities;
using Profile.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Profile.Persistence;

public sealed class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options)
    {
    }

    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("profile");
        modelBuilder.ApplyConfiguration(new UserSettingsConfiguration());
    }
}
