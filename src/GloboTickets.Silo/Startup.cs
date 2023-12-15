using Azure.Monitor.OpenTelemetry.AspNetCore;
using GloboTickets.Silo.ConfigureOptions;
using GloboTickets.Silo.HealthChecks;
using HealthChecks.ApplicationStatus.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;

namespace GloboTickets.Silo;

public class Startup
{
    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment webHostEnvironment;

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration, where key value pair settings are stored (See
    /// http://docs.asp.net/en/latest/fundamentals/configuration.html).</param>
    /// <param name="webHostEnvironment">The environment the application is running under. This can be Development,
    /// Staging or Production by default (See http://docs.asp.net/en/latest/fundamentals/environments.html).</param>
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        this.configuration = configuration;
        this.webHostEnvironment = webHostEnvironment;
    }

    public virtual void ConfigureServices(IServiceCollection services) =>
    services
            .ConfigureOptions<ConfigureRequestLoggingOptions>()
            .ConfigureAspNetCoreInstrumentation()
            .ConfigureOpenTelemetryMeterProvider((sp, metricConfig) =>
                metricConfig.AddOrleansMeters(webHostEnvironment)
                    .AddPrometheusExporter())
            .ConfigureOpenTelemetryTracerProvider((sp, tracingConfig) =>
                tracingConfig
                    .ConfigureResource(resource =>
                        resource.ConfigureDefaultResource(webHostEnvironment))
                    .AddOrleansTracing(webHostEnvironment))
            .AddHealthChecks()
                .AddApplicationStatus()
                .AddCheck<ClusterHealthCheck>(nameof(ClusterHealthCheck))
                .AddCheck<GrainHealthCheck>(nameof(GrainHealthCheck))
                .AddCheck<SiloHealthCheck>(nameof(SiloHealthCheck))
                .AddCheck<StorageHealthCheck>(nameof(StorageHealthCheck))
            .Services
                .AddOpenTelemetry()
                .UseAzureMonitor()
                .WithTracing(builder =>
                {
                    builder.AddIf(
                        webHostEnvironment.IsDevelopment(),
                        x => x.AddConsoleExporter(
                            options => options.Targets = ConsoleExporterOutputTargets.Console | ConsoleExporterOutputTargets.Debug));
                })
            .Services
                .AddRouting();

    public virtual void Configure(IApplicationBuilder application) =>
        application
            .UseRouting()
            .UseSerilogRequestLogging()
            .UseEndpoints(
                builder =>
                {
                    builder.MapPrometheusScrapingEndpoint();
                    builder.MapDefaultHealthChecks();
                });

    private static StorageOptions GetStorageOptions(IConfiguration configuration) =>
        configuration.GetSection(nameof(ApplicationOptions.Storage)).Get<StorageOptions>();
}