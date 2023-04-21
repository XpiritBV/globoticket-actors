public interface IConcertGrain : IGrainWithGuidKey
{
    Task<Order> BuyTicket(int quantity);
    Task SetAmountofTickets(int amount);
}

public class ConcertGrain: Grain, IGrainWithGuidKey{

    private int _availableTickets;

    public Task<Order> BuyTicket(int quantity)
    {
        return Task.FromResult(new Order());
    }

    public Task SetAmountofTickets(int amount)
    {
        _availableTickets = amount;
    }
}

[GenerateSerializer, Immutable]
public class ConcertState
{
    public int AvailableTickets { get; set; }
}