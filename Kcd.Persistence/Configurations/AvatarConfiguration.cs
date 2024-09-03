using Kcd.Common.Enums;
using Kcd.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kcd.Persistence.Configurations;

public class AvatarConfiguration : IEntityTypeConfiguration<Avatar>
{
    public void Configure(EntityTypeBuilder<Avatar> builder)
    {
        builder.HasKey(ua => ua.Id);
        builder.Property(ua => ua.FileName).IsRequired().HasMaxLength(255);
        builder.Property(ua => ua.ContentType).IsRequired().HasMaxLength(50);
        builder.Property(a => a.StorageStrategy)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (AvatarStorageStrategy)Enum.Parse(typeof(AvatarStorageStrategy), v));
    }
}
