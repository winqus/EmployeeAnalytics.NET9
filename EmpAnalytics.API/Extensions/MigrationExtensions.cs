using EmpAnalytics.Persistence;
using EmpAnalytics.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }

    public static async Task SeedDatabaseAsync(this WebApplication app, bool useLargeSeed = true)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await DatabaseSeeder.SeedAsync(dbContext, useLargeSeed);
    }
}