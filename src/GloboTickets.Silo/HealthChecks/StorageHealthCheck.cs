namespace GloboTickets.Silo.HealthChecks;

/// <summary>
/// Verifies whether the <see cref="IStorageHealthCheckGrain"/> can read, write and clear state using the default
/// storage provider.
/// </summary>
public class StorageHealthCheck : IHealthCheck
{
    private const string FailedMessage = "Failed storage health check.";
    private readonly IClusterClient _client;
    private readonly IOptionsMonitor<SiloOptions> _siloOptions;
    private readonly ILogger<StorageHealthCheck> _logger;

    public StorageHealthCheck(
        IClusterClient client,
        IOptionsMonitor<SiloOptions> siloOptions,
        ILogger<StorageHealthCheck> logger)
    {
        _client = client;
        _siloOptions = siloOptions;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Call this grain with a the silo name as a key each time. This grain then deactivates itself, so there is a new
            // instance created and destroyed each time.
            await _client.GetGrain<IStorageHealthCheckGrain>(_siloOptions.CurrentValue.SiloName).CheckAsync().ConfigureAwait(false);
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
        {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            _logger.FailedStorageHealthCheck(exception);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            return HealthCheckResult.Unhealthy(FailedMessage, exception);
        }

        return HealthCheckResult.Healthy();
    }
}