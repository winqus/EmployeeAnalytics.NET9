namespace EmpAnalytics.Application.Users.Get;

public interface IGetTop5EmployeesExceedingOwnAverage
{
    Task<IReadOnlyList<EmployeeExceedingOwnAverageResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
}