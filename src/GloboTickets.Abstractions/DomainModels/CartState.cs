namespace GloboTickets.Abstractions.DomainModels;

[GenerateSerializer, Immutable]
public class CartState
{
    [Id(0)]
    public Dictionary<string, CartItem> ShoppingCart { get; set; }
}