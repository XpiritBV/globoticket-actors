namespace GloboTickets.Abstractions.DomainModels;

[GenerateSerializer, Immutable]
public class ConcertState
{
    [Id(0)]
    public HashSet<string> TicketIds { get; set; }
}