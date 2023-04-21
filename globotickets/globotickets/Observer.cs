using Orleans.Utilities;

namespace globotickets;

public interface IConcertWatcher : IGrainObserver
{
    Task UpdateTicketSale(int ticketsLeft);
}