namespace GloboTickets.Grains.HealthChecks;

[StatelessWorker(1)]
public class LocalHealthCheckGrain : Grain, ILocalHealthCheckGrain
{
    public ValueTask CheckAsync() => ValueTask.CompletedTask;
}