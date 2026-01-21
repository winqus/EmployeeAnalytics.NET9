using EmpAnalytics.Application.Users.Get;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.Persistence.Services;

public sealed class GetTop5EmployeesExceedingOwnAverage : IGetTop5EmployeesExceedingOwnAverage
{
    private readonly ApplicationDbContext _context;

    public GetTop5EmployeesExceedingOwnAverage(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<EmployeeExceedingOwnAverageResponse>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var last30Days = now.AddDays(-30);
        var last6Months = now.AddMonths(-6);

        return await _context.Users
            .AsNoTracking()
            .Select(u => new
            {
                User = u,
                Last30DaysCount = _context.UserJobs
                    .Count(uj => uj.UserId == u.UserId && uj.DateTimeCreated >= last30Days),
                SixMonthAverage = _context.UserJobs
                    .Where(uj => uj.UserId == u.UserId && uj.DateTimeCreated >= last6Months)
                    .Count() / 6.0
            })
            .Where(x => x.Last30DaysCount > x.SixMonthAverage)
            .OrderByDescending(x => x.Last30DaysCount)
            .Take(5)
            .Select(x => new EmployeeExceedingOwnAverageResponse(
                x.User.Username,
                x.User.Lastname,
                x.Last30DaysCount))
            .ToListAsync(cancellationToken);
    }
}