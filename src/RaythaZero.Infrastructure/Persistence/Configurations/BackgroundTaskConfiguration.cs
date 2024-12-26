using RaythaZero.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaythaZero.Domain.ValueObjects;

namespace RaythaZero.Infrastructure.Persistence.Configurations;

public class BackgroundTaskConfiguration : IEntityTypeConfiguration<BackgroundTask>
{
    public void Configure(EntityTypeBuilder<BackgroundTask> builder)
    {
        builder
            .Property(b => b.Status)
            .HasConversion(v => v.DeveloperName, v => BackgroundTaskStatus.From(v));
    }
}
