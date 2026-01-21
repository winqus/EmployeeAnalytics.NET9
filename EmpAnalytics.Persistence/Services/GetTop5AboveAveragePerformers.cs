using EmpAnalytics.Application.Users.Get;
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
        var jobCountsPerUser = await _context.UserJobs
            .AsNoTracking()
            .Where(uj => uj.DateTimeCreated >= startDate && uj.DateTimeCreated <= endDate)
            .GroupBy(uj => uj.UserId)
            .Select(g => new { UserId = g.Key, JobCount = g.Count() })
            .ToListAsync(cancellationToken);

        if (jobCountsPerUser.Count == 0)
            return [];

        var averageJobCount = jobCountsPerUser.Average(x => x.JobCount);

        var topUserIds = jobCountsPerUser
            .Where(x => x.JobCount > averageJobCount)
            .OrderByDescending(x => x.JobCount)
            .Take(5)
            .ToList();

        var userIds = topUserIds.Select(x => x.UserId).ToList();

        var users = await _context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.UserId))
            .ToDictionaryAsync(u => u.UserId, cancellationToken);

        return topUserIds
            .Select(x => new AboveAveragePerformerResponse(
                users[x.UserId].Username,
                users[x.UserId].Lastname,
                x.JobCount))
            .ToList();
    }
}