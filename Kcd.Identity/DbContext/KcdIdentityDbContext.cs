﻿using Kcd.Common.Enums;
using Kcd.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kcd.Identity.DbContext;

/// <summary>
/// Represents the Identity database context using the KcdUser entity.
/// </summary>
public class KcdIdentityDbContext : IdentityDbContext<KcdUser>
{
    public KcdIdentityDbContext(DbContextOptions<KcdIdentityDbContext> options)
        : base(options)
    {
    }

    // Additional configuration for the KcdUser model
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(KcdIdentityDbContext).Assembly);

        builder.Entity<KcdUser>(b => { b.ToTable("Users"); });
        builder.Entity<IdentityUserClaim<string>>(b => { b.ToTable("UserClaims"); });
        builder.Entity<IdentityUserLogin<string>>(b => { b.ToTable("UserLogins"); });
        builder.Entity<IdentityUserToken<string>>(b => { b.ToTable("UserTokens"); });
        builder.Entity<KcdRole>(b => { b.ToTable("Roles"); });
        builder.Entity<IdentityRoleClaim<string>>(b => { b.ToTable("RoleClaims"); });
        builder.Entity<IdentityUserRole<string>>(b => { b.ToTable("UserRoles"); });

        // Seed data
        SeedData(builder);
    }

    private void SeedData(ModelBuilder builder)
    {
        foreach (string role in Enum.GetNames(typeof(Roles)))
        {
            var guid = Guid.NewGuid().ToString();
            builder.Entity<KcdRole>().HasData(
                new KcdRole(role)
                {
                    Description = role,
                    NormalizedName = role.ToUpper(),
                    Id = guid
                });

            if (role == Common.Enums.Roles.Admin.ToString())
            {
                SeedAdminUser(builder, guid);
            }
        }
    }

    private void SeedAdminUser(ModelBuilder builder, string roleGuid)
    {
        var userGuid = Guid.NewGuid().ToString();
        // Seed admin user
        var adminUser = new KcdUser
        {
            Id = userGuid,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM",
            EmailConfirmed = true,
            Name = "Admin User",
            Country = "N/A",
            Company = "KCD",
            Referral = "Internal",
            AvatarUrl = string.Empty // Assuming this is optional
        };

        adminUser.PasswordHash = GeneratePasswordHash(adminUser, "Admin*123"); //TODO: change this hard coded password
        builder.Entity<KcdUser>().HasData(adminUser);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
        {
            RoleId = roleGuid,
            UserId = userGuid
        });
    }

    public string GeneratePasswordHash(KcdUser user, string password)
    {
        var passHash = new PasswordHasher<KcdUser>();
        return passHash.HashPassword(user, password);
    }
}