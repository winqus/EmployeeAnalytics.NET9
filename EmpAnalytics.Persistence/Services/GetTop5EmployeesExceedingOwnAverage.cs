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
        return await _context.Database
            .SqlQueryRaw<EmployeeExceedingOwnAverageResponse>("EXEC GetTop5EmployeesExceedingOwnAverage")
            .ToListAsync(cancellationToken);
    }
}