using globotickets;
using Orleans;

public class ConcertWatcher : BackgroundService, IConcertWatcher
{
    private readonly IGrainFactory factory;

    public ConcertWatcher(IGrainFactory factory)
    {
        this.factory = factory;
    }

    public Task UpdateTicketSale(int ticketsLeft)
    {
        Console.WriteLine($"Number of tickets left {ticketsLeft}");
        return Task.CompletedTask;
    }

    protected override  Task ExecuteAsync(CancellationToken stoppingToken)
    {

        Task.Run(async () =>
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                var friend = factory.GetGrain<IConcertGrain>("taylorswift");

                //Create a reference for chat, usable for subscribing to the observable grain.
                var obj = factory.CreateObjectReference<IConcertWatcher>(this);

                //Subscribe the instance to receive messages.
                await friend.Subscribe(obj);
            }
        }, stoppingToken);

        return Task.CompletedTask;
        
    }
}