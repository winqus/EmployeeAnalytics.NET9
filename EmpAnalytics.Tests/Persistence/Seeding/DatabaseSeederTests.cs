using EmpAnalytics.Persistence.Seeding;
using EmpAnalytics.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.Tests.Persistence.Seeding
{
    public class DatabaseSeederTests
    {
        private static ApplicationDbContext CreateContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task SeedAsync_DoesNotThrow()
        {
            using var context = CreateContext();

            var exception = await Record.ExceptionAsync(() =>
                DatabaseSeeder.SeedAsync(context, useLargeSeed: false)
            );

            Assert.Null(exception);
        }
        
        [Fact]
        public async Task SeedAsync_With1M_DoesNotThrow()
        {
            using var context = CreateContext();

            var exception = await Record.ExceptionAsync(() =>
                DatabaseSeeder.SeedAsync(context, useLargeSeed: true)
            );

            Assert.Null(exception);
        }

        [Fact]
        public async Task SeedAsync_InsertsExpectedCounts()
        {
            using var context = CreateContext();

            await DatabaseSeeder.SeedAsync(context, useLargeSeed: false);

            Assert.Equal(30, await context.Jobs.CountAsync());
            Assert.Equal(100, await context.Users.CountAsync());
            Assert.Equal(10_000, await context.UserJobs.CountAsync());
        }

        [Fact]
        public async Task SeedAsync_WhenCalledTwice_DoesNotDuplicate()
        {
            using var context = CreateContext();

            await DatabaseSeeder.SeedAsync(context, useLargeSeed: false);
            await DatabaseSeeder.SeedAsync(context, useLargeSeed: false);

            Assert.Equal(30, await context.Jobs.CountAsync());
            Assert.Equal(100, await context.Users.CountAsync());
            Assert.Equal(10_000, await context.UserJobs.CountAsync());
        }

        [Fact]
        public async Task SeedAsync_DoesNotCreateDuplicateCompositeKeys()
        {
            using var context = CreateContext();

            await DatabaseSeeder.SeedAsync(context, useLargeSeed: false);

            var duplicateCount =
                await context.UserJobs
                    .GroupBy(x => new { x.UserId, x.JobId, x.DateTimeCreated })
                    .Where(g => g.Count() > 1)
                    .CountAsync();

            Assert.Equal(0, duplicateCount);
        }

        [Fact]
        public async Task SeedAsync_UserJobsHaveValidForeignKeys()
        {
            using var context = CreateContext();

            await DatabaseSeeder.SeedAsync(context, useLargeSeed: false);

            var invalidUserJobs =
                await context.UserJobs
                    .Where(uj =>
                        !context.Users.Any(u => u.UserId == uj.UserId) ||
                        !context.Jobs.Any(j => j.JobId == uj.JobId))
                    .CountAsync();

            Assert.Equal(0, invalidUserJobs);
        }
    }
}
