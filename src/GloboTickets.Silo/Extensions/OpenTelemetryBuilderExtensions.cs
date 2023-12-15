using GloboTickets.Silo.OpenTelemetry;
using OpenTelemetry.Resources;

namespace GloboTickets.Silo.Extensions;

public static partial class OpenTelemetryBuilderExtensions
{
    public static ResourceBuilder ConfigureDefaultResource(
        this ResourceBuilder builder,
        IWebHostEnvironment webHostEnvironment)
    {
        builder.AddService(
                webHostEnvironment.ApplicationName)
            .AddAttributes(
                new KeyValuePair<string, object>[]
                {
                    new(OpenTelemetryAttributeName.Deployment.Environment, webHostEnvironment.EnvironmentName),
                    new(OpenTelemetryAttributeName.Host.Name, Environment.MachineName),
                })
            .AddEnvironmentVariableDetector();

        return builder;
    }
}