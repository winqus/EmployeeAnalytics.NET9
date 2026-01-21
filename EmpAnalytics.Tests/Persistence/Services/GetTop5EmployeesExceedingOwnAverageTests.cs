using EmpAnalytics.Domain.UserJobs;
using EmpAnalytics.Domain.Users;
using EmpAnalytics.Persistence.Services;

namespace EmpAnalytics.Tests.Persistence.Services;

public sealed class GetTop5EmployeesExceedingOwnAverageTests : InMemoryBaseIntegrationTest
{
    [Fact]
    public async Task ExecuteAsync_UserExceedingOwnAverage_IsReturned()
    {
        var context = CreateContext(nameof(ExecuteAsync_UserExceedingOwnAverage_IsReturned));

        var userId = TestUtils.CreateGuid(1);
        var jobId = TestUtils.CreateGuid(100);
        var now = DateTime.UtcNow;

        context.Users.Add(new User(userId, "John", "Doe"));

        /* 6 jobs placed between 31 days and 6 months ago (average = 1 per month) */
        for (int i = 0; i < 6; i++)
        {
            context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-31 - i * 25)));
        }

        /* 3 jobs in last 30 days (exceeds average of 1) */
        for (int i = 0; i < 3; i++)
        {
            context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-i)));
        }

        await context.SaveChangesAsync();

        var sut = new GetTop5EmployeesExceedingOwnAverage(context);

        var result = await sut.ExecuteAsync();

        Assert.Single(result);
        Assert.Equal("John", result[0].FirstName);
        Assert.Equal("Doe", result[0].LastName);
        Assert.Equal(3, result[0].JobCountLast30Days);
    }

    [Fact]
    public async Task ExecuteAsync_UserAtOrBelowAverage_IsExcluded()
    {
        var context = CreateContext(nameof(ExecuteAsync_UserAtOrBelowAverage_IsExcluded));

        var userId = TestUtils.CreateGuid(1);
        var jobId = TestUtils.CreateGuid(100);
        var now = DateTime.UtcNow;

        context.Users.Add(new User(userId, "John", "Doe"));

        /* 12 jobs placed between 31 days and 6 months ago (outside last 30 days) 
           Average = 12 / 6.0 = 2 per month */
        for (int i = 0; i < 12; i++)
        {
            context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-31 - i * 10)));
        }

        /* 2 jobs in last 30 days (equals average of 2, not exceeding) */
        context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-1)));
        context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-2)));

        await context.SaveChangesAsync();

        var sut = new GetTop5EmployeesExceedingOwnAverage(context);

        var result = await sut.ExecuteAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_NoUserJobs_ReturnsEmpty()
    {
        var context = CreateContext(nameof(ExecuteAsync_NoUserJobs_ReturnsEmpty));

        context.Users.Add(new User(TestUtils.CreateGuid(1), "John", "Doe"));
        await context.SaveChangesAsync();

        var sut = new GetTop5EmployeesExceedingOwnAverage(context);

        var result = await sut.ExecuteAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsMaximumFiveUsers()
    {
        var context = CreateContext(nameof(ExecuteAsync_ReturnsMaximumFiveUsers));

        var jobId = TestUtils.CreateGuid(100);
        var now = DateTime.UtcNow;

        for (int i = 1; i <= 10; i++)
        {
            var userId = TestUtils.CreateGuid(i);
            context.Users.Add(new User(userId, $"User{i}", $"Last{i}"));

            /* 6 jobs placed between 31 days and 6 months ago (average = 1) */
            for (int m = 0; m < 6; m++)
            {
                context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-31 - m * 25)));
            }

            /* i+1 jobs in last 30 days (all exceed average of 1) */
            for (int j = 0; j <= i; j++)
            {
                context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-j)));
            }
        }

        await context.SaveChangesAsync();

        var sut = new GetTop5EmployeesExceedingOwnAverage(context);

        var result = await sut.ExecuteAsync();

        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsOrderedByJobCountDescending()
    {
        var context = CreateContext(nameof(ExecuteAsync_ReturnsOrderedByJobCountDescending));

        var jobId = TestUtils.CreateGuid(100);
        var now = DateTime.UtcNow;

        /* Create 3 users with different job counts in last 30 days */
        var jobCounts = new[] { 5, 10, 3 };

        for (int i = 0; i < 3; i++)
        {
            var userId = TestUtils.CreateGuid(i + 1);
            context.Users.Add(new User(userId, $"User{i + 1}", $"Last{i + 1}"));

            /* 6 jobs placed between 31 days and 6 months ago (average = 1) */
            for (int m = 0; m < 6; m++)
            {
                context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-31 - m * 25)));
            }

            /* Variable jobs in last 30 days */
            for (int j = 0; j < jobCounts[i]; j++)
            {
                context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-j)));
            }
        }

        await context.SaveChangesAsync();

        var sut = new GetTop5EmployeesExceedingOwnAverage(context);

        var result = await sut.ExecuteAsync();

        Assert.Equal(3, result.Count);

        for (int i = 0; i < result.Count - 1; i++)
        {
            Assert.True(result[i].JobCountLast30Days >= result[i + 1].JobCountLast30Days);
        }

        Assert.Equal(10, result[0].JobCountLast30Days);
        Assert.Equal(5, result[1].JobCountLast30Days);
        Assert.Equal(3, result[2].JobCountLast30Days);
    }

    [Fact]
    public async Task ExecuteAsync_UserWithNoHistoricalJobs_ButRecentJobs_IsReturned()
    {
        var context = CreateContext(nameof(ExecuteAsync_UserWithNoHistoricalJobs_ButRecentJobs_IsReturned));

        var userId = TestUtils.CreateGuid(1);
        var jobId = TestUtils.CreateGuid(100);
        var now = DateTime.UtcNow;

        context.Users.Add(new User(userId, "NewUser", "Smith"));

        /* Only recent jobs, no historical jobs (average = 0, any job exceeds it) */
        context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-1)));

        await context.SaveChangesAsync();

        var sut = new GetTop5EmployeesExceedingOwnAverage(context);

        var result = await sut.ExecuteAsync();

        Assert.Single(result);
        Assert.Equal("NewUser", result[0].FirstName);
        Assert.Equal(1, result[0].JobCountLast30Days);
    }

    [Fact]
    public async Task ExecuteAsync_JobsOutsideSixMonths_AreNotCountedInAverage()
    {
        var context = CreateContext(nameof(ExecuteAsync_JobsOutsideSixMonths_AreNotCountedInAverage));

        var userId = TestUtils.CreateGuid(1);
        var jobId = TestUtils.CreateGuid(100);
        var now = DateTime.UtcNow;

        context.Users.Add(new User(userId, "John", "Doe"));

        /* Many jobs older than 6 months (should not affect average) */
        for (int i = 0; i < 100; i++)
        {
            context.UserJobs.Add(new UserJob(userId, jobId, now.AddMonths(-7).AddDays(-i)));
        }

        /* 1 job in last 30 days, no jobs in 6-month window except this
           Average = 0.167, so 1 job exceeds it */
        context.UserJobs.Add(new UserJob(userId, jobId, now.AddDays(-1)));

        await context.SaveChangesAsync();

        var sut = new GetTop5EmployeesExceedingOwnAverage(context);

        var result = await sut.ExecuteAsync();

        Assert.Single(result);
        Assert.Equal(1, result[0].JobCountLast30Days);
    }
}