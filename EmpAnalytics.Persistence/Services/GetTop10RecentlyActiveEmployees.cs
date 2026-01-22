using EmpAnalytics.Application.Users.Get;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.Persistence.Services;

public sealed class GetTop10RecentlyActiveEmployees : IGetTop10RecentlyActiveEmployees
{
    private readonly ApplicationDbContext _context;

    public GetTop10RecentlyActiveEmployees(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RecentlyActiveEmployeeResponse>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Database
            .SqlQueryRaw<RecentlyActiveEmployeeResponse>("EXEC GetTop10RecentlyActiveEmployees")
            .ToListAsync(cancellationToken);
    }
}