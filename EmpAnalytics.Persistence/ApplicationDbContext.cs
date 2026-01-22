using EmpAnalytics.Application.Data;
using EmpAnalytics.Application.Users.Get;
using EmpAnalytics.Domain.Jobs;
using EmpAnalytics.Domain.UserJobs;
using EmpAnalytics.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EmpAnalytics.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<RecentlyActiveEmployeeResponse>().HasNoKey().ToView(null);
        modelBuilder.Entity<AboveAveragePerformerResponse>().HasNoKey().ToView(null);
        modelBuilder.Entity<EmployeeExceedingOwnAverageResponse>().HasNoKey().ToView(null);
    }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserJob> UserJobs { get; set; }
}