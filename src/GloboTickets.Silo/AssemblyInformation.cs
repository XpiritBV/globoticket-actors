using System.Reflection;

namespace GloboTickets.Silo;

public record AssemblyInformation(string Product, string Description, string Version)
{
    public static readonly AssemblyInformation Current = new(typeof(AssemblyInformation).Assembly);

    public AssemblyInformation(Assembly assembly)
        : this(
            assembly.GetCustomAttribute<AssemblyProductAttribute>()!.Product,
            assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()!.Description,
            assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()!.Version)
    {
    }
}
