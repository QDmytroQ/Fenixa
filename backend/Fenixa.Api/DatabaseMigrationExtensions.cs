using Content.Persistence;
using Identity.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Profile.Persistence;
using Study.Persistence;

namespace Fenixa.Api;

public static class DatabaseMigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        await EnsureDatabaseExistsAsync(app.Configuration);

        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        await serviceProvider.GetRequiredService<IdentityDbContext>().Database.MigrateAsync();
        await serviceProvider.GetRequiredService<ProfileDbContext>().Database.MigrateAsync();
        await serviceProvider.GetRequiredService<ContentDbContext>().Database.MigrateAsync();
        await serviceProvider.GetRequiredService<StudyDbContext>().Database.MigrateAsync();
    }

    private static async Task EnsureDatabaseExistsAsync(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is missing.");

        var builder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = builder.InitialCatalog;
        builder.InitialCatalog = "master";

        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = $"""
            IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'{databaseName}')
            BEGIN
                CREATE DATABASE [{databaseName}];
            END
            """;
        await command.ExecuteNonQueryAsync();
    }
}
