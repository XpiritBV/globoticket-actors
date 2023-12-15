using System.Reflection;

namespace GloboTickets.Silo.Extensions;

/// <summary>
/// <see cref="IConfigurationBuilder"/> extension methods.
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
    /// used to conditionally add to the configuration pipeline.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder.</param>
    /// <param name="condition">If set to <c>true</c> the action is executed.</param>
    /// <param name="action">The action used to add to the request execution pipeline.</param>
    /// <returns>The same configuration builder.</returns>
    public static IConfigurationBuilder AddIf(
        this IConfigurationBuilder configurationBuilder,
        bool condition,
        Func<IConfigurationBuilder, IConfigurationBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            configurationBuilder = action(configurationBuilder);
        }

        return configurationBuilder;
    }

    /// <summary>
    /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
    /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
    /// the configuration pipeline.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder.</param>
    /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
    /// <paramref name="elseAction"/> is executed.</param>
    /// <param name="ifAction">The action used to add to the configuration pipeline if the condition is
    /// <c>true</c>.</param>
    /// <param name="elseAction">The action used to add to the configuration pipeline if the condition is
    /// <c>false</c>.</param>
    /// <returns>The same configuration builder.</returns>
    public static IConfigurationBuilder AddIfElse(
        this IConfigurationBuilder configurationBuilder,
        bool condition,
        Func<IConfigurationBuilder, IConfigurationBuilder> ifAction,
        Func<IConfigurationBuilder, IConfigurationBuilder> elseAction)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);
        ArgumentNullException.ThrowIfNull(ifAction);
        ArgumentNullException.ThrowIfNull(elseAction);

        configurationBuilder = condition ? ifAction(configurationBuilder) : elseAction(configurationBuilder);

        return configurationBuilder;
    }

    public static IConfigurationBuilder AddCustomBootstrapConfiguration(
        this IConfigurationBuilder configurationBuilder, string[]? args) =>
        configurationBuilder
            .AddEnvironmentVariables(prefix: "DOTNET_")
            .AddIf(
                args is not null,
                x => x.AddCommandLine(args!));

    public static IConfigurationBuilder AddCustomConfiguration(
        this IConfigurationBuilder configurationBuilder,
        IHostEnvironment hostEnvironment,
        string[]? args) =>
        configurationBuilder
            // Add configuration from the appsettings.json file.
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            // Add configuration from an optional appsettings.development.json, appsettings.staging.json or
            // appsettings.production.json file, depending on the environment. These settings override the ones in
            // the appsettings.json file.
            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            // Add configuration from an optional appsettings.k8s.json file. These settings override the ones in
            // the appsettings.json file.
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "configmap", $"appsettings.k8s.json"), optional: true, reloadOnChange: true)
            // Add configuration from files in the specified directory. The name of the file is the key and the
            // contents the value.
            .AddKeyPerFile(Path.Combine(Directory.GetCurrentDirectory(), "secrets"), optional: true, reloadOnChange: true)
            // This reads the configuration keys from the secret store. This allows you to store connection strings
            // and other sensitive settings, so you don't have to check them into your source control provider.
            // Only use this in Development, it is not intended for Production use. See
            // http://docs.asp.net/en/latest/security/app-secrets.html
            .AddIf(
                hostEnvironment.IsDevelopment() && !string.IsNullOrEmpty(hostEnvironment.ApplicationName),
                x => x.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: false))
            // Add configuration specific to the Development, Staging or Production environments. This config can
            // be stored on the machine being deployed to or if you are using Azure, in the cloud. These settings
            // override the ones in all of the above config files. See
            // http://docs.asp.net/en/latest/security/app-secrets.html
            .AddEnvironmentVariables()
            // Add command line options. These take the highest priority.
            .AddIf(
                args is not null,
                x => x.AddCommandLine(args!));
}