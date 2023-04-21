using Orleans.Runtime;

public interface IConcertGrain : IGrainWithGuidKey
{
    Task<Order> BuyTicket(int quantity);
    Task BeginTicketSale(int ticketAmount);
    Task GetAvailableTicketAmount();
}

public class ConcertGrain : Grain, IConcertGrain
{

    private readonly IPersistentState<ConcertState> _concert;

    public ConcertGrain(
        [PersistentState(
            stateName: nameof(ConcertState),
            storageName: "concerts")]
        IPersistentState<ConcertState> concertState)
    {
        _concert = concertState;
    }

    public async Task<Order> BuyTicket(int quantity)
    {
        _concert.State = _concert.State with { AvailableTickets = _concert.State.AvailableTickets - quantity };
        await _concert.WriteStateAsync();
        return new Order(quantity);
    }

    public async Task BeginTicketSale(int amount)
    {
        _concert.State.AvailableTickets = amount;
        await _concert.WriteStateAsync();
    }

    public Task GetAvailableTicketAmount()
    {
        return Task.FromResult(_concert.State.AvailableTickets);
    }
}

[GenerateSerializer, Immutable]
public sealed record ConcertState
{
    public int AvailableTickets { get; set; }
}