using EmpAnalytics.Application.Data;
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
    }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserJob> UserJobs { get; set; }
}