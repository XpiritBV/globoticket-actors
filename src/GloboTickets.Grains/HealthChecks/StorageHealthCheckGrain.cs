namespace GloboTickets.Grains.HealthChecks;

[PreferLocalPlacement]
public class StorageHealthCheckGrain : Grain<StorageHealthCheckGrainState>, IStorageHealthCheckGrain
{
    public async ValueTask CheckAsync()
    {
        try
        {
            this.State = new StorageHealthCheckGrainState { Id = Guid.NewGuid() };
            await this.WriteStateAsync().ConfigureAwait(true);
            await this.ReadStateAsync().ConfigureAwait(true);
            await this.ClearStateAsync().ConfigureAwait(true);
        }
        finally
        {
            this.DeactivateOnIdle();
        }
    }
}

[GenerateSerializer]
public record StorageHealthCheckGrainState
{
    [Id(0)]
    public Guid Id { get; set; }
}