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
    return await (
            from uj in _context.UserJobs.AsNoTracking()
            join u in _context.Users.AsNoTracking() on uj.UserId equals u.UserId
            join j in _context.Jobs.AsNoTracking() on uj.JobId equals j.JobId
            orderby uj.DateTimeCreated descending
            select new RecentlyActiveEmployeeResponse(
                u.Username,
                u.Lastname,
                j.Name,
                uj.DateTimeCreated))
        .Take(10)
        .ToListAsync(cancellationToken);
  }
}