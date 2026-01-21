namespace EmpAnalytics.Application.Users.Get;

public interface IGetTop10RecentlyActiveEmployees
{
    Task<IReadOnlyList<RecentlyActiveEmployeeResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
}