using RaythaZero.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RaythaZero.Infrastructure.Persistence.Configurations;

public class JwtLoginConfiguration : IEntityTypeConfiguration<JwtLogin>
{
    public void Configure(EntityTypeBuilder<JwtLogin> builder)
    {
        builder.HasIndex(b => b.Jti).IsUnique();
    }
}
