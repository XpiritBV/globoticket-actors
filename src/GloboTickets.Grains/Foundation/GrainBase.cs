namespace GloboTickets.Grains.Foundation;

public abstract class GrainBase<T> : GrainBase
{
    protected readonly IPersistentState<T> state;
    protected T State => state.State;

    protected GrainBase(IPersistentState<T> state) => this.state = state;
}

public abstract class GrainBase : Grain
{
    internal T GetGrain<T>(Guid id) where T : IGrainWithStringKey =>
        this.GrainFactory.GetGrain<T>(id.ToString());

    internal T GetGrain<T>(string id) where T : IGrainWithStringKey =>
        this.GrainFactory.GetGrain<T>(id);
}