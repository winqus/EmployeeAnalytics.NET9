using EmpAnalytics.Domain.Jobs;
using EmpAnalytics.Domain.UserJobs;
using EmpAnalytics.Domain.Users;
using EmpAnalytics.Persistence.Services;

namespace EmpAnalytics.Tests.Persistence.Services;

public sealed class GetTop10RecentlyActiveEmployeesTests : InMemoryBaseIntegrationTest
{
    [Fact]
    public async Task ExecuteAsync_ReturnsEmployeesOrderedByMostRecent()
    {
        var context = CreateContext(nameof(ExecuteAsync_ReturnsEmployeesOrderedByMostRecent));

        var userId = TestUtils.CreateGuid(1);
        var jobId = TestUtils.CreateGuid(100);

        context.Users.Add(new User(userId, "John", "Doe"));
        context.Jobs.Add(new Job(jobId, "Developer"));

        var dates = new[]
        {
            new DateTime(2024, 1, 15),
            new DateTime(2024, 1, 10),
            new DateTime(2024, 1, 20)
        };

        foreach (var date in dates)
        {
            context.UserJobs.Add(new UserJob(userId, jobId, date));
        }

        await context.SaveChangesAsync();

        var sut = new GetTop10RecentlyActiveEmployees(context);

        var result = await sut.ExecuteAsync();

        Assert.Equal(3, result.Count);

        for (int i = 0; i < result.Count - 1; i++)
        {
            Assert.True(result[i].LastJobDate >= result[i + 1].LastJobDate);
        }

        Assert.Equal(new DateTime(2024, 1, 20), result[0].LastJobDate);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsMaximumTenEmployees()
    {
        var context = CreateContext(nameof(ExecuteAsync_ReturnsMaximumTenEmployees));

        var jobId = TestUtils.CreateGuid(100);
        context.Jobs.Add(new Job(jobId, "Developer"));

        var baseDate = new DateTime(2024, 1, 1);

        for (int i = 1; i <= 15; i++)
        {
            var userId = TestUtils.CreateGuid(i);
            context.Users.Add(new User(userId, $"User{i}", $"Last{i}"));
            context.UserJobs.Add(new UserJob(userId, jobId, baseDate.AddDays(i)));
        }

        await context.SaveChangesAsync();

        var sut = new GetTop10RecentlyActiveEmployees(context);

        var result = await sut.ExecuteAsync();

        Assert.Equal(10, result.Count);
    }

    [Fact]
    public async Task ExecuteAsync_NoUserJobs_ReturnsEmpty()
    {
        var context = CreateContext(nameof(ExecuteAsync_NoUserJobs_ReturnsEmpty));

        var sut = new GetTop10RecentlyActiveEmployees(context);

        var result = await sut.ExecuteAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsCorrectUserAndJobData()
    {
        var context = CreateContext(nameof(ExecuteAsync_ReturnsCorrectUserAndJobData));

        var userId = TestUtils.CreateGuid(1);
        var jobId = TestUtils.CreateGuid(100);
        var jobDate = new DateTime(2024, 5, 15, 10, 30, 0);

        context.Users.Add(new User(userId, "Alice", "Smith"));
        context.Jobs.Add(new Job(jobId, "Senior Engineer"));
        context.UserJobs.Add(new UserJob(userId, jobId, jobDate));

        await context.SaveChangesAsync();

        var sut = new GetTop10RecentlyActiveEmployees(context);

        var result = await sut.ExecuteAsync();

        Assert.Single(result);

        var employee = result[0];
        Assert.Equal("Alice", employee.FirstName);
        Assert.Equal("Smith", employee.LastName);
        Assert.Equal("Senior Engineer", employee.JobName);
        Assert.Equal(jobDate, employee.LastJobDate);
    }

    [Fact]
    public async Task ExecuteAsync_MultipleUsersWithMultipleJobs_ReturnsCorrectOrder()
    {
        var context = CreateContext(nameof(ExecuteAsync_MultipleUsersWithMultipleJobs_ReturnsCorrectOrder));

        var user1Id = TestUtils.CreateGuid(1);
        var user2Id = TestUtils.CreateGuid(2);
        var job1Id = TestUtils.CreateGuid(100);
        var job2Id = TestUtils.CreateGuid(101);

        context.Users.AddRange(
            new User(user1Id, "User1", "Last1"),
            new User(user2Id, "User2", "Last2")
        );

        context.Jobs.AddRange(
            new Job(job1Id, "Job1"),
            new Job(job2Id, "Job2")
        );

        context.UserJobs.AddRange(
            new UserJob(user1Id, job1Id, new DateTime(2024, 1, 10)),
            new UserJob(user1Id, job2Id, new DateTime(2024, 1, 20)),
            new UserJob(user2Id, job1Id, new DateTime(2024, 1, 15)),
            new UserJob(user2Id, job2Id, new DateTime(2024, 1, 25))
        );

        await context.SaveChangesAsync();

        var sut = new GetTop10RecentlyActiveEmployees(context);

        var result = await sut.ExecuteAsync();

        Assert.Equal(4, result.Count);

        Assert.Equal("User2", result[0].FirstName);
        Assert.Equal("Job2", result[0].JobName);
        Assert.Equal(new DateTime(2024, 1, 25), result[0].LastJobDate);

        Assert.Equal("User1", result[1].FirstName);
        Assert.Equal("Job2", result[1].JobName);
        Assert.Equal(new DateTime(2024, 1, 20), result[1].LastJobDate);
    }
}