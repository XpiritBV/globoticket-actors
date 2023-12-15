
namespace GloboTickets.Grains;

public class ConcertTicketGrain : GrainBase<TicketDetails>, IConcertTicketGrain
{
    public ConcertTicketGrain(
        [PersistentState(
            stateName: "Tickets")]
        IPersistentState<TicketDetails> state) : base(state) { }

    Task<int> IConcertTicketGrain.GetAvailabilityAsync() =>
        Task.FromResult(State.Quantity);

    Task<TicketDetails> IConcertTicketGrain.GetTicketDetailsAsync() =>
        Task.FromResult(State);

    Task IConcertTicketGrain.ReturnTicketAsync(int quantity) =>
        UpdateStateAsync(State with
        {
            Quantity = State.Quantity + quantity
        });

    async Task<(bool IsAvailable, TicketDetails? TicketDetails)> IConcertTicketGrain.TryBuyTicketsAsync(int quantity)
    {
        if (State.Quantity < quantity)
        {
            return (false, null);
        }

        var updatedState = State with
        {
            Quantity = State.Quantity - quantity
        };

        await UpdateStateAsync(updatedState);

        return (true, State);
    }

    Task IConcertTicketGrain.CreateOrUpdateTicketAsync(TicketDetails ticket) =>
        UpdateStateAsync(ticket);

    private async Task UpdateStateAsync(TicketDetails ticket)
    {
        state.State = ticket;
        await state.WriteStateAsync();
    }
}