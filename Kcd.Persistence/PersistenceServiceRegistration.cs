using Kcd.Persistence.DatabaseContext;
using Kcd.Persistence.HealthChecks;
using Kcd.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Internal;

namespace Kcd.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<UserApplicationDatabaseContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("UserApplicationDatabase"));
        });

        services.AddSingleton<ISystemClock, SystemClock>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserApplicationRepository, UserApplicationRepository>();
        services.AddScoped<IAvatarRepository, AvatarRepository>();

        return services;
    }

    public static IServiceCollection AddPersistenceHealthCheks(this IServiceCollection services)
    {
        // Register persistence health checks
        services.AddHealthChecks()
            .AddCheck<SqliteHealthCheck>("SQLite Health Check", HealthStatus.Unhealthy, new[] { "db", "sqlite" });

        return services;
    }
}
