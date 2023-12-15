using GloboTickets.Abstractions.DomainModels;

namespace GloboTickets.Abstractions.Grains;

public interface IConcertTicketGrain : IGrainWithStringKey
{
    Task<(bool IsAvailable, TicketDetails? TicketDetails)> TryBuyTicketsAsync(int quantity);
    Task ReturnTicketAsync(int quantity);
    Task<int> GetAvailabilityAsync();

    Task CreateOrUpdateTicketAsync(TicketDetails productDetails);
    Task<TicketDetails> GetTicketDetailsAsync();
}