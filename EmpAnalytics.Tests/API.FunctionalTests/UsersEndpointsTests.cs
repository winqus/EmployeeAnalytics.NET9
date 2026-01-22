using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using EmpAnalytics.Application.Users.Get;

namespace EmpAnalytics.Tests.API.FunctionalTests;

public class UsersEndpointsTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    private const string BaseUrl = "api/v1/employees";
    private const int MaxResponseTimeSeconds = 2;

    private static void AssertResponseTimeWithinLimit(Stopwatch stopwatch) =>
        Assert.True(
            stopwatch.Elapsed <= TimeSpan.FromSeconds(MaxResponseTimeSeconds),
            $"Request took {stopwatch.Elapsed.TotalMilliseconds} ms, which exceeds {MaxResponseTimeSeconds} seconds");

    [Fact]
    public async Task Should_ReturnRecentlyActiveEmployeesWithin2seconds()
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await HttpClient.GetAsync($"{BaseUrl}/recently-active");

        stopwatch.Stop();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var employees =
            await response.Content.ReadFromJsonAsync<List<RecentlyActiveEmployeeResponse>>();
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.Equal(10, employees.Count);
        AssertResponseTimeWithinLimit(stopwatch);
    }

    [Fact]
    public async Task Should_ReturnAboveAveragePerformersWithin2seconds()
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await HttpClient.GetAsync($"{BaseUrl}/above-average");

        stopwatch.Stop();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var employees =
            await response.Content.ReadFromJsonAsync<List<AboveAveragePerformerResponse>>();
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.Equal(5, employees.Count);
        AssertResponseTimeWithinLimit(stopwatch);
    }

    [Fact]
    public async Task Should_ReturnAboveAveragePerformersWithDateRangeWithin2seconds()
    {
        var startDate = DateTime.UtcNow.AddMonths(-6).ToString("O");
        var endDate = DateTime.UtcNow.ToString("O");
        var stopwatch = Stopwatch.StartNew();

        var response = await HttpClient.GetAsync(
            $"{BaseUrl}/above-average?startDate={startDate}&endDate={endDate}");

        stopwatch.Stop();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var employees =
            await response.Content.ReadFromJsonAsync<List<AboveAveragePerformerResponse>>();
        Assert.NotNull(employees);
        AssertResponseTimeWithinLimit(stopwatch);
    }

    [Fact]
    public async Task Should_ReturnEmployeesExceedingOwnAverageWithin2seconds()
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await HttpClient.GetAsync($"{BaseUrl}/exceeding-own-average");

        stopwatch.Stop();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var employees =
            await response.Content.ReadFromJsonAsync<List<EmployeeExceedingOwnAverageResponse>>();
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.Equal(5, employees.Count);
        AssertResponseTimeWithinLimit(stopwatch);
    }
}