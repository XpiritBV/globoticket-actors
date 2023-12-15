using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GloboTickets.Silo.Options;

public class ApplicationOptions
{
    public ClusterOptions Cluster { get; set; } = default!;

    public KestrelServerOptions Kestrel { get; set; } = default!;

    public StorageOptions Storage { get; set; } = default!;
}