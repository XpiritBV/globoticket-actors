namespace GloboTickets.Abstractions.Grains.HealthChecks;

public interface ILocalHealthCheckGrain : IGrainWithGuidKey
{
    ValueTask CheckAsync();
}