using EmpAnalytics.Domain.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmpAnalytics.Persistence.Configurations;

internal class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(j => j.JobId);

        builder.Property(j => j.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(j => j.Name);
    }
}