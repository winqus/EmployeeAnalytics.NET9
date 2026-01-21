using EmpAnalytics.Domain.Jobs;
using EmpAnalytics.Domain.Users;
using EmpAnalytics.Domain.UserJobs;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Job> Jobs { get; set; }

    DbSet<User> Users { get; set; }

    DbSet<UserJob> UserJobs { get; set; }
}