using RaythaZero.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RaythaZero.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder
            .HasIndex(b => b.Category);

        builder
            .HasIndex(b => b.CreationTime);

        builder
            .HasIndex(b => b.EntityId);
    }
}
