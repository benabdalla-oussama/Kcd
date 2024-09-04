using Kcd.Common.HealthChecks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Kcd.Identity.HealthChecks;

/// <summary>
/// Performs a health check for a SQL Server database connection.
/// </summary>
/// <remarks>
/// This class checks the connectivity and health of the SQL Server database specified in the application's configuration.
/// It inherits from <see cref="DefaultHealthCheck"/> and uses the connection string named "IdentityDatabase" from the configuration.
/// </remarks>
public class SqlServerHealthCheck(IConfiguration configuration) : DefaultHealthCheck("SQL Server")
{
    private readonly string _connectionString = configuration.GetConnectionString("IdentityDatabase");

    public override async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return HealthCheckResult.Healthy("SQL Server is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("SQL Server is unhealthy. Exception: " + ex.Message);
        }
    }
}