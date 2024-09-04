using Kcd.Application;
using Kcd.Identity;
using Kcd.Infrastructure;
using Kcd.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Kcd.Api
{
    public static class Extensions
    {
        /// <summary>
        /// Registers application, infrastructure, persistence, and identity services required by the API.
        /// </summary>
        public static IHostApplicationBuilder ApiServiesRegistration(this IHostApplicationBuilder builder)
        {
            // Add required services
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Applies pending migrations for both Identity and User Application databases.
        /// </summary>
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            // Apply migrations for Identity database context
            services.ApplyIdentityMigrations();

            // Apply migrations for User application database context
            services.ApplyPersistenceMigrations();
        }

        /// <summary>
        /// Adds health check services to the application, including persistence and identity health checks.
        /// </summary>
        public static IHostApplicationBuilder AddHealthChecks(this IHostApplicationBuilder builder)
        {
            // Add Health Checks
            builder.Services.AddPersistenceHealthCheks();
            builder.Services.AddIdentityHealthCheks();
            return builder;
        }

        /// <summary>
        /// Maps default health check endpoints for the application, including general and liveness health checks.
        /// </summary>
        public static WebApplication MapDefaultEndpoints(this WebApplication app)
        {
            // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
            // app.MapPrometheusScrapingEndpoint();

            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });

            return app;
        }
    }
}
