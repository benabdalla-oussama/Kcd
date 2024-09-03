using Kcd.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kcd.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<KcdUser>
{
    public void Configure(EntityTypeBuilder<KcdUser> builder)
    {
        // Configure custom properties for KcdUser
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Country).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Company).HasMaxLength(100);
        builder.Property(e => e.Referral).HasMaxLength(100);
        builder.Property(e => e.AvatarUrl).HasMaxLength(250);
    }
}
