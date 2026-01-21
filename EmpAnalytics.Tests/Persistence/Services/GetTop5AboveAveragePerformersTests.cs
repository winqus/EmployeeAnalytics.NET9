using EmpAnalytics.Domain.UserJobs;
using EmpAnalytics.Domain.Users;
using EmpAnalytics.Persistence.Services;

namespace EmpAnalytics.Tests.Persistence.Services;

public sealed class GetTop5AboveAveragePerformersTests : InMemoryBaseIntegrationTest
{
    [Fact]
    public async Task ExecuteAsync_ReturnsTop5AboveAveragePerformers()
    {
        var context = CreateContext(nameof(ExecuteAsync_ReturnsTop5AboveAveragePerformers));

        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 31);
        var jobId = TestUtils.CreateGuid(100);

        context.Users.AddRange(
            new User(TestUtils.CreateGuid(1), "u1", "L1"),
            new User(TestUtils.CreateGuid(2), "u2", "L2"),
            new User(TestUtils.CreateGuid(3), "u3", "L3"),
            new User(TestUtils.CreateGuid(4), "u4", "L4"),
            new User(TestUtils.CreateGuid(5), "u5", "L5"),
            new User(TestUtils.CreateGuid(6), "u6", "L6")
        );

        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(1), jobId, startDate));

        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(2), jobId, startDate));
        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(2), jobId, startDate.AddHours(1)));

        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(3), jobId, startDate));
        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(3), jobId, startDate.AddHours(1)));
        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(3), jobId, startDate.AddHours(2)));

        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(4), jobId, startDate));
        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(4), jobId, startDate.AddHours(1)));
        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(4), jobId, startDate.AddHours(2)));
        context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(4), jobId, startDate.AddHours(3)));

        for (int i = 0; i < 5; i++)
        {
            context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(5), jobId, startDate.AddHours(i)));
        }

        await context.SaveChangesAsync();

        var sut = new GetTop5AboveAveragePerformers(context);

        var result = await sut.ExecuteAsync(startDate, endDate);

        Assert.Equal(2, result.Count);

        for (int i = 0; i < result.Count - 1; i++)
        {
            Assert.True(result[i].JobCount >= result[i + 1].JobCount);
        }

        var usernames = result.Select(x => x.FirstName).ToList();
        Assert.Contains("u4", usernames);
        Assert.Contains("u5", usernames);
    }

    [Fact]
    public async Task ExecuteAsync_NoJobsInRange_ReturnsEmpty()
    {
        var context = CreateContext(nameof(ExecuteAsync_NoJobsInRange_ReturnsEmpty));
        var sut = new GetTop5AboveAveragePerformers(context);

        var result = await sut.ExecuteAsync(
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow);

        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_UsersWithAverageJobCount_AreExcluded()
    {
        var context = CreateContext(nameof(ExecuteAsync_UsersWithAverageJobCount_AreExcluded));
        var jobId = TestUtils.CreateGuid(100);

        context.Users.AddRange(
            new User(TestUtils.CreateGuid(1), "u1", "L1"),
            new User(TestUtils.CreateGuid(2), "u2", "L2")
        );

        context.UserJobs.AddRange(
            new UserJob(TestUtils.CreateGuid(1), jobId, DateTime.Today),
            new UserJob(TestUtils.CreateGuid(2), jobId, DateTime.Today)
        );

        await context.SaveChangesAsync();

        var sut = new GetTop5AboveAveragePerformers(context);

        var result = await sut.ExecuteAsync(
            DateTime.Today.AddDays(-1),
            DateTime.Today.AddDays(1));

        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_Returns_Maximum_Of_Five_Users()
    {
        var context = CreateContext(nameof(ExecuteAsync_Returns_Maximum_Of_Five_Users));

        var startDate = DateTime.Today;
        var jobId = TestUtils.CreateGuid(100);

        for (int i = 1; i <= 10; i++)
        {
            context.Users.Add(new User(TestUtils.CreateGuid(i), $"u{i}", $"L{i}"));

            for (int j = 0; j < i; j++)
            {
                context.UserJobs.Add(new UserJob(TestUtils.CreateGuid(i), jobId, startDate.AddHours(j)));
            }
        }

        await context.SaveChangesAsync();

        var sut = new GetTop5AboveAveragePerformers(context);

        var result = await sut.ExecuteAsync(
            startDate.AddDays(-1),
            startDate.AddDays(1));

        Assert.Equal(5, result.Count);

        for (int i = 0; i < result.Count - 1; i++)
        {
            Assert.True(result[i].JobCount >= result[i + 1].JobCount);
        }
    }
}