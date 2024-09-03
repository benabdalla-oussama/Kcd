using Kcd.Identity;
using Kcd.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Kcd.Api
{
    public static class Extensions
    {
        public static IHostApplicationBuilder ApiServiesRegistration(this IHostApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            return builder;
        }

        public static IHostApplicationBuilder AddHealthChecks(this IHostApplicationBuilder builder)
        {
            // Add Health Checks
            builder.Services.AddPersistenceHealthCheks();
            builder.Services.AddIdentityHealthCheks();
            return builder;
        }

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
