using OpenTelemetry.Metrics;

namespace GloboTickets.Silo.Extensions;

public static partial class MeterProviderBuilderExtensions
{
    public static MeterProviderBuilder AddIf(
        this MeterProviderBuilder builder,
        bool condition,
        Func<MeterProviderBuilder, MeterProviderBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            builder = action(builder);
        }

        return builder;
    }

    public static MeterProviderBuilder AddOrleansMeters(
        this MeterProviderBuilder builder,
        IWebHostEnvironment webHostEnvironment) =>
        builder
            .AddMeter("Microsoft.Orleans");
}
