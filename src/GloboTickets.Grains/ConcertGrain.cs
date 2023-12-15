namespace GloboTickets.Grains;

[Reentrant]
public class ConcertGrain : GrainBase<ConcertState>, IConcertGrain
{
    private readonly Dictionary<string, TicketDetails> _ticketCache = new();

    public ConcertGrain([PersistentState(
        stateName: "Concert")]
        IPersistentState<ConcertState> state) : base(state) {}

    public override Task OnActivateAsync(CancellationToken cancellationToken) => 
        PopulateTicketCacheAsync(cancellationToken);

    Task<HashSet<TicketDetails>> IConcertGrain.GetAllTicketsAsync() => 
        Task.FromResult(new HashSet<TicketDetails>(_ticketCache.Values));

    async Task IConcertGrain.AddOrUpdateTicketAsync(TicketDetails ticket)
    {
        var internalTicketId = GetTicketIdForConcert(ticket.Id);
        ticket.Id = internalTicketId;
        State.TicketIds.Add(internalTicketId);
        _ticketCache[internalTicketId] = ticket;

        var ticketGrain = GetGrain<IConcertTicketGrain>(internalTicketId);
        await ticketGrain.CreateOrUpdateTicketAsync(ticket);

        await state.WriteStateAsync();
    }

    async Task IConcertGrain.RemoveTicketAsync(string ticketId)
    {
        var internalTicketId = GetTicketIdForConcert(ticketId);
        State.TicketIds.Remove(internalTicketId);
        _ticketCache.Remove(internalTicketId);

        await state.WriteStateAsync();
    }

    private async Task PopulateTicketCacheAsync(CancellationToken cancellationToken)
    {
        if (State is not { TicketIds.Count: > 0 })
        {
            return;
        }

        await Parallel.ForEachAsync(
            State.TicketIds,
            new ParallelOptions
            {
                TaskScheduler = TaskScheduler.Current,
                CancellationToken = cancellationToken
            },
            async (id, _) =>
            {
                var ticketGrain = GrainFactory.GetGrain<IConcertTicketGrain>(id);
                _ticketCache[id] = await ticketGrain.GetTicketDetailsAsync();
            });
    }

    private string GetTicketIdForConcert(string ticketId)
    {
        return ticketId.Contains(this.GetPrimaryKeyString()) ? ticketId : $"{this.GetPrimaryKeyString()}_{ticketId}";
    }
}
