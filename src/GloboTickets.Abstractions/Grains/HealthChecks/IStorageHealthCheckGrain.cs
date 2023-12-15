namespace GloboTickets.Abstractions.Grains.HealthChecks;

public interface IStorageHealthCheckGrain : IGrainWithStringKey
{
    ValueTask CheckAsync();
}