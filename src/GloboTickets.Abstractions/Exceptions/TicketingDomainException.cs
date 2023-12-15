namespace GloboTickets.Abstractions.Exceptions;

[GenerateSerializer]
public class TicketingDomainException : Exception
{
    public TicketingDomainException() { }
    public TicketingDomainException(string message) : base(message) { }
    public TicketingDomainException(string message, Exception innerException) : base(message, innerException) { }
}
