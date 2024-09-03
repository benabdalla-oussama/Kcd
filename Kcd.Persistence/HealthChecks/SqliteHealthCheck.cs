using Kcd.Common.HealthChecks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Kcd.Persistence.HealthChecks;

public class SqliteHealthCheck(IConfiguration configuration) : DefaultHealthCheck("SQLite")
{
    private readonly string _connectionString = configuration.GetConnectionString("HrDatabaseConnectionString");

    public override async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return HealthCheckResult.Healthy("SQLite is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("SQLite is unhealthy. Exception: " + ex.Message);
        }
    }
}