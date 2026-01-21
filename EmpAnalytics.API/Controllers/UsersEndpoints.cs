using EmpAnalytics.Application.Users.Get;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EmpAnalytics.API.Controllers;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/employees");

        group.MapGet("recently-active", GetRecentlyActiveEmployees)
            .WithName(nameof(GetRecentlyActiveEmployees));

        group.MapGet("above-average", GetAboveAveragePerformers)
            .WithName(nameof(GetAboveAveragePerformers));

        group.MapGet("exceeding-own-average", GetEmployeesExceedingOwnAverage)
            .WithName(nameof(GetEmployeesExceedingOwnAverage));
    }

    public static async Task<Results<Ok<List<RecentlyActiveEmployeeResponse>>, IResult>> GetRecentlyActiveEmployees(
        IGetTop10RecentlyActiveEmployees service,
        CancellationToken ct)
    {
        var employees = await service.ExecuteAsync(ct);

        return TypedResults.Ok(employees);
    }

    public static async Task<Results<Ok<List<AboveAveragePerformerResponse>>, IResult>> GetAboveAveragePerformers(
        IGetTop5AboveAveragePerformers service,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken ct)
    {
        var start = startDate ?? DateTime.UtcNow.AddMonths(-3);
        var end = endDate ?? DateTime.UtcNow;
        var employees = await service.ExecuteAsync(start, end, ct);

        return TypedResults.Ok(employees);
    }

    public static async Task<Results<Ok<List<EmployeeExceedingOwnAverageResponse>>, IResult>>
        GetEmployeesExceedingOwnAverage(
            IGetTop5EmployeesExceedingOwnAverage service,
            CancellationToken ct)
    {
        var employees = await service.ExecuteAsync(ct);

        return TypedResults.Ok(employees);
    }
}