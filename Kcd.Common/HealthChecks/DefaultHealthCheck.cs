using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Kcd.Common.HealthChecks;

/// <summary>
/// An abstract base class for implementing custom health checks in an application.
/// </summary>
/// <remarks>
/// This class provides a foundation for creating health checks by encapsulating common functionality and managing the health check name.
/// Derived classes must implement the <see cref="CheckHealthAsync"/> method to define the actual health check logic.
/// </remarks>
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