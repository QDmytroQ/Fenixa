using Study.Domain.Entities;
using Study.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Study.Persistence;

public sealed class StudyDbContext : DbContext
{
    public StudyDbContext(DbContextOptions<StudyDbContext> options) : base(options)
    {
    }

    public DbSet<CardProgress> CardProgress => Set<CardProgress>();
    public DbSet<UserStatistics> UserStatistics => Set<UserStatistics>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("study");
        modelBuilder.ApplyConfiguration(new CardProgressConfiguration());
        modelBuilder.ApplyConfiguration(new UserStatisticsConfiguration());
    }
}
