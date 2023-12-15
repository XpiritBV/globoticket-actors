using GloboTickets.Abstractions.DomainModels;

namespace GloboTickets.Abstractions.Grains;

public interface IConcertGrain : IGrainWithStringKey
{
    [ReadOnly]
    Task<HashSet<TicketDetails>> GetAllTicketsAsync();
    Task AddOrUpdateTicketAsync(TicketDetails ticketDetails);
    Task RemoveTicketAsync(string ticketId);
}