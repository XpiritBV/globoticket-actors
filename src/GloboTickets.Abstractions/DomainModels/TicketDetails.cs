namespace GloboTickets.Abstractions.DomainModels;

[GenerateSerializer, Immutable]
public sealed record TicketDetails
{
    [Id(0)]
    public string Id { get; set; } = null!;
    [Id(1)]
    public string Name { get; set; } = null!;
    [Id(2)]
    public string Description { get; set; } = null!;
    [Id(3)]
    public int Quantity { get; set; }
    [Id(4)]
    public decimal UnitPrice { get; set; }


    [JsonIgnore]
    public decimal TotalPrice =>
        Math.Round(Quantity * UnitPrice, 2);
}