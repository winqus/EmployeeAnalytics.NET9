using EmpAnalytics.Application.Users.Get;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.Persistence.Services;

public sealed class GetTop5AboveAveragePerformers : IGetTop5AboveAveragePerformers
{
    private readonly ApplicationDbContext _context;

    public GetTop5AboveAveragePerformers(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AboveAveragePerformerResponse>> ExecuteAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Database
            .SqlQueryRaw<AboveAveragePerformerResponse>(
                "EXEC GetTop5AboveAveragePerformers {0}, {1}",
                startDate,
                endDate)
            .ToListAsync(cancellationToken);
    }
}