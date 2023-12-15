using Serilog.Extensions.Hosting;
using Serilog;
using System.Globalization;

namespace GloboTickets.Silo.Extensions;

public static class SerilogExtensions
{
    public static bool SerilogConfigurationAvailable(IConfiguration configuration)
    {
        var serilogSection = configuration.GetSection("Serilog");
        return serilogSection.Exists();
    }

    /// <summary>
    /// Creates a logger used during application initialisation.
    /// <see href="https://nblumhardt.com/2020/10/bootstrap-logger/"/>.
    /// </summary>
    /// <returns>A logger that can load a new configuration.</returns>
    public static ReloadableLogger CreateBootstrapLogger() =>
        new LoggerConfiguration()
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.Debug(formatProvider: CultureInfo.InvariantCulture)
            .CreateBootstrapLogger();

    /// <summary>
    /// Configures a logger used during the applications lifetime.
    /// <see href="https://nblumhardt.com/2020/10/bootstrap-logger/"/>.
    /// </summary>
    public static void ConfigureReloadableLogger(
        Microsoft.Extensions.Hosting.HostBuilderContext context,
        IServiceProvider services,
        LoggerConfiguration configuration)
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);

        
        configuration
            .WriteTo.Conditional(
                x => context.HostingEnvironment.IsDevelopment(),
                x => x
                    .Console(formatProvider: CultureInfo.InvariantCulture)
                    .WriteTo
                    .Debug(formatProvider: CultureInfo.InvariantCulture));
    }

}