using globotickets;
using Orleans.Runtime;
using Orleans.Utilities;
using System;

public interface IConcertGrain : IGrainWithStringKey
{
    Task<Order> BuyTicket(int quantity);
    Task BeginTicketSale(int ticketAmount);
    Task GetAvailableTicketAmount();
    Task<ConcertState> GetConcertState();

    Task Subscribe(IConcertWatcher observer);
    Task UnSubscribe(IConcertWatcher observer);
}

// [GrainLabel("globotickets")]
public class ConcertGrain : Grain, IConcertGrain
{
    private readonly ObserverManager<IConcertWatcher> _subsManager;

    //private globotickets.IGrainObserver _observer;
    private readonly IPersistentState<ConcertState> _concert;

    public ConcertGrain(
        [PersistentState(
            stateName: nameof(ConcertState),
            storageName: "concerts")]
        IPersistentState<ConcertState> concertState, ILogger<ConcertGrain> logger)
    {
        _concert = concertState;
        _subsManager = new ObserverManager<IConcertWatcher>(
        TimeSpan.FromMinutes(5), logger);
    }

    public async Task<Order> BuyTicket(int quantity)
    {
        if (_concert.State.AvailableTickets - quantity < 0)
        {
            throw new InvalidOperationException("Not enough tickets available");
        }

        _concert.State = _concert.State with { AvailableTickets = _concert.State.AvailableTickets - quantity };
        await _concert.WriteStateAsync();

        await _subsManager.Notify(s => s.UpdateTicketSale(_concert.State.AvailableTickets));

        return new Order(quantity);
    }

    public async Task BeginTicketSale(int amount)
    {
        _concert.State.AvailableTickets = amount;
        await _concert.WriteStateAsync();
    }

    public Task GetAvailableTicketAmount()
    {
        return Task.FromResult(_concert.State.AvailableTickets);
    }
    public Task<ConcertState> GetConcertState() =>  Task.FromResult(_concert.State);

    public Task Subscribe(IConcertWatcher observer)
    {
        _subsManager.Subscribe(observer, observer);
        return Task.CompletedTask;
    }

    public Task UnSubscribe(IConcertWatcher observer)
    {
        _subsManager.Unsubscribe(observer);
        return Task.CompletedTask;
    }
}

[GenerateSerializer, Immutable]
public sealed record ConcertState
{
    public int AvailableTickets { get; set; }
}