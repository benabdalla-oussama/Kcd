using Kcd.Common.HealthChecks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Kcd.Identity.HealthChecks;

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