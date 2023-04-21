using Orleans.Core.Internal;
using Orleans.Hosting;
using Orleans.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<ConcertWatcher>();

builder.Host.UseOrleans(
    builder => builder
        .UseLocalhostClustering()
        .AddAzureTableGrainStorage(
            name: "concerts",
            configureOptions: options =>
            {
                options.ConfigureTableServiceClient(
                    "UseDevelopmentStorage=true");
                //                    "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==");
            })
        .UseDashboard(options => { })
    ) ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/beginticketsale", (IGrainFactory factory, string concertId) =>
{
    var concert = factory.GetGrain<IConcertGrain>(concertId);
    concert.BeginTicketSale(100);
})
.WithName("BeginTicketSale").WithOpenApi();

app.MapGet("/buyTicket", async (IGrainFactory factory, int quantity, string concertId) =>
{
    var concert = factory.GetGrain<IConcertGrain>(concertId);
    await concert.BuyTicket(quantity);
})
.WithName("BuyTicket")
.WithOpenApi();

// app.MapGet("/concerts", async (IGrainFactory factory) =>
// {
//     var concerts = factory.GetGrain<IGrainObserver>();
    
// })
// .WithName("Concerts")
// .WithOpenApi();

app.Run();

