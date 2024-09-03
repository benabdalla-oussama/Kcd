using Kcd.Common.Enums;
using Kcd.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kcd.Persistence.Configurations;

public class UserApplicationConfiguration : IEntityTypeConfiguration<UserApplication>
{
    public void Configure(EntityTypeBuilder<UserApplication> builder)
    {
        builder.HasKey(ua => ua.Id);
        builder.Property(ua => ua.Name).IsRequired().HasMaxLength(100);
        builder.Property(ua => ua.Email).IsRequired().HasMaxLength(100);
        builder.Property(ua => ua.Country).IsRequired().HasMaxLength(50);
        builder.Property(ua => ua.Company).HasMaxLength(100);
        builder.Property(ua => ua.Referral).HasMaxLength(100);
        builder.Property(ua => ua.Status).HasDefaultValue(ApplicationStatus.Pending);
    }
}
