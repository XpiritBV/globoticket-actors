namespace GloboTickets.Grains;

[Reentrant]
public class ShoppingCartGrain : GrainBase<CartState>, IShoppingCartGrain
{

    public ShoppingCartGrain(
        [PersistentState(
            stateName: "ShoppingCart")]
        IPersistentState<CartState> state) : base(state){}


    async Task<bool> IShoppingCartGrain.AddOrUpdateItemAsync(int quantity, TicketDetails ticket)
    {
        var tickets = GetGrain<IConcertTicketGrain>(ticket.Id);

        int? adjustedQuantity = null;
        if (State.ShoppingCart.TryGetValue(ticket.Id, out var existingItem))
        {
            adjustedQuantity = quantity - existingItem.Quantity;
        }

        var (isAvailable, claimedTicket) =
            await tickets.TryBuyTicketsAsync(adjustedQuantity ?? quantity);

        if (isAvailable && claimedTicket is not null)
        {
            var item = ToCartItem(quantity, claimedTicket);
            State.ShoppingCart[claimedTicket.Id] = item;

            await state.WriteStateAsync();
            return true;
        }

        return false;
    }

    Task IShoppingCartGrain.EmptyCartAsync()
    {
        State.ShoppingCart.Clear();
        return state.ClearStateAsync();
    }

    Task<HashSet<CartItem>> IShoppingCartGrain.GetAllItemsAsync() =>
        Task.FromResult(State.ShoppingCart.Values.ToHashSet());

    Task<int> IShoppingCartGrain.GetTotalItemsInCartAsync() =>
        Task.FromResult(State.ShoppingCart.Count);

    async Task IShoppingCartGrain.RemoveItemAsync(TicketDetails ticket)
    {
        var tickets = GrainFactory.GetGrain<IConcertTicketGrain>(ticket.Id);
        await tickets.ReturnTicketAsync(ticket.Quantity);

        if (State.ShoppingCart.Remove(ticket.Id))
        {
            await state.WriteStateAsync();
        }
    }

    private CartItem ToCartItem(int quantity, TicketDetails ticket) =>
        new(this.GetPrimaryKeyString(), quantity, ticket);
}