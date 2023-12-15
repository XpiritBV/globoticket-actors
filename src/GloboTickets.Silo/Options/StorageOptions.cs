namespace GloboTickets.Silo.Options;

public class StorageOptions
{
    public string ConnectionString { get; set; } = default!;
    public bool UseAzureIdentity { get; set; } = default!;
}
