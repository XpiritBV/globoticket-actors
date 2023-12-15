using GloboTickets.Abstractions.DomainModels;

namespace GloboTickets.Abstractions.Grains;

public interface IShoppingCartGrain : IGrainWithStringKey
{
    /// <summary>
    /// Adds the given <paramref name="quantity"/> of the corresponding
    /// <paramref name="ticket"/> to the shopping cart.
    /// </summary>
    Task<bool> AddOrUpdateItemAsync(int quantity, TicketDetails ticket);

    /// <summary>
    /// Removes the given <paramref name="ticket" /> from the shopping cart.
    /// </summary>
    Task RemoveItemAsync(TicketDetails ticket);

    /// <summary>
    /// Gets all the items in the shopping cart.
    /// </summary>
    Task<HashSet<CartItem>> GetAllItemsAsync();

    /// <summary>
    /// Gets the number of items in the shopping cart.
    /// </summary>
    Task<int> GetTotalItemsInCartAsync();

    /// <summary>
    /// Removes all items from the shopping cart.
    /// </summary>
    Task EmptyCartAsync();
}