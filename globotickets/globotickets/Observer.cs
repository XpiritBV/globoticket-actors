using Orleans.Utilities;

namespace globotickets;


public interface IConcertWatcher : IGrainObserver
{
    Task UpdateTicketSale(int ticketsLeft);
}

public class ConcertWatcher2 : IConcertWatcher
{
    public Task UpdateTicketSale(int ticketsLeft)
    {
        Console.WriteLine($"Number of tickets left {ticketsLeft}");
        return Task.CompletedTask;
    }
}