using EmpAnalytics.Domain.Jobs;
using EmpAnalytics.Domain.UserJobs;
using EmpAnalytics.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmpAnalytics.Persistence.Configurations;

internal class UserJobConfiguration : IEntityTypeConfiguration<UserJob>
{
    public void Configure(EntityTypeBuilder<UserJob> builder)
    {
        builder.HasKey(uj => new { uj.UserId, uj.JobId, uj.DateTimeCreated });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(uj => uj.UserId)
            .IsRequired();

        builder.HasOne<Job>()
            .WithMany()
            .HasForeignKey(uj => uj.JobId)
            .IsRequired();

        builder.HasIndex(uj => new { uj.UserId, uj.DateTimeCreated })
            .IsDescending(false, true);
    }
}