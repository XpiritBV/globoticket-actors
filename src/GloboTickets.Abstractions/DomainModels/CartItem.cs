namespace GloboTickets.Abstractions.DomainModels;

[GenerateSerializer, Immutable]
public sealed record class CartItem(
    string UserId,
    int Quantity,
    TicketDetails Ticket)
{
    [JsonIgnore]
    public decimal TotalPrice =>
        Math.Round(Quantity * Ticket.UnitPrice, 2);
}