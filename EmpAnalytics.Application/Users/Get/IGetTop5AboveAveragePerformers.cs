namespace EmpAnalytics.Application.Users.Get;

public interface IGetTop5AboveAveragePerformers
{
    Task<IReadOnlyList<AboveAveragePerformerResponse>> ExecuteAsync(DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default);
}