using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Kcd.Common.HealthChecks;

public abstract class DefaultHealthCheck : IHealthCheck
{
    private readonly string _name;

    protected DefaultHealthCheck(string name)
    {
        _name = name;
    }

    public abstract Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default);

    protected HealthCheckResult Healthy()
    {
        return HealthCheckResult.Healthy($"{_name} is healthy.");
    }

    protected HealthCheckResult Unhealthy(string reason)
    {
        return HealthCheckResult.Unhealthy($"{_name} is unhealthy. Reason: {reason}");
    }
}