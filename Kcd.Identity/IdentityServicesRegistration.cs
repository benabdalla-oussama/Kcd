using Kcd.Identity.DatabaseContext;
using Kcd.Identity.Entities;
using Kcd.Identity.HealthChecks;
using Kcd.Identity.Helpers;
using Kcd.Identity.Models;
using Kcd.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Kcd.Identity;

public static class IdentityServicesRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(options =>
            configuration.GetSection(JwtSettings.SectionKey).Bind(options));

        services.AddDbContext<IdentityDatabaseContext>(options =>
           options.UseSqlServer(configuration.GetConnectionString("IdentityDatabase"), x => x.EnableRetryOnFailure()));

        services.AddIdentity<KcdUser, KcdRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = true;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<IdentityDatabaseContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton<ISystemClock, SystemClock>();
        services.AddTransient<IAuthService, AuthService>();

        services.AddJwtAuthentication(configuration)
            .AddAuthorizationPolicies()
            .AddAuthorizationBuilder();

        return services;
    }

    public static void ApplyIdentityMigrations(this IServiceProvider services)
    {
        // Apply migrations for Identity database context
        var identityDbContext = services.GetRequiredService<IdentityDatabaseContext>();
        identityDbContext.Database.Migrate();
    }

    public static IServiceCollection AddIdentityHealthCheks(this IServiceCollection services)
    {
        // Register identity health checks
        services.AddHealthChecks()
            .AddCheck<SqlServerHealthCheck>("SQL Server Health Check", HealthStatus.Unhealthy, new[] { "db", "sql" });

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
            };
        });

        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(config =>
        {
            config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
            config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
        });

        return services;
    }
}
