using Serilog;

static StorageOptions GetStorageOptions(IConfiguration configuration) =>
    configuration.GetSection(nameof(ApplicationOptions.Storage)).Get<StorageOptions>();

static IHostBuilder CreateHostBuilder(string[] args) =>
        new HostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .AddBuilderDefaults(typeof(Program).Assembly, args)
            .UseOrleans(ConfigureSiloBuilder)
            .ConfigureWebHost(ConfigureWebHostBuilder)
            .UseDefaultServiceProvider(
                (context, options) =>
                {
                    var isDevelopment = context.HostingEnvironment.IsDevelopment();
                    options.ValidateScopes = isDevelopment;
                    options.ValidateOnBuild = isDevelopment;
                })
            .UseConsoleLifetime();

static void ConfigureSiloBuilder(HostBuilderContext context,
    ISiloBuilder siloBuilder) =>
    siloBuilder
        .ConfigureDefaultSiloBuilder(context)
        .ConfigureServices(
            (services) =>
            {
                services.Configure<ApplicationOptions>(context.Configuration);
                services.Configure<ClusterOptions>(
                    context.Configuration.GetSection(nameof(ApplicationOptions.Cluster)));
                services.Configure<StorageOptions>(
                    context.Configuration.GetSection(nameof(ApplicationOptions.Storage)));

            })
        .UseIf(
            context.HostingEnvironment.IsDevelopment(),
            (x) =>
            {
                x.UseLocalhostClustering();
                x.UseInMemoryReminderService();
                x.AddMemoryGrainStorageAsDefault();
                return x;
            })
        .UseDashboard(configure =>
        {
            configure.Port = 8082;
        });

static void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder) =>
    webHostBuilder
        .UseStartup<Startup>()
        .UseKestrel(
            (builderContext, options) =>
            {
                options.AddServerHeader = false;
                options.Configure(
                    builderContext.Configuration.GetSection(nameof(ApplicationOptions.Kestrel)),
                    reloadOnChange: false);
            })
;

var builder = CreateHostBuilder(args);

var app = builder.Build();

Log.Logger = SerilogExtensions.CreateBootstrapLogger();

try
{
    Log.Information("Initialising.");
    app.LogApplicationStarted();
    await app.RunAsync().ConfigureAwait(false);
    app.LogApplicationStopped();
    return 0;
}
catch (Exception exception)
{
    app!.LogApplicationTerminatedUnexpectedly(exception);

    return 1;
}
finally
{
    await Log.CloseAndFlushAsync().ConfigureAwait(false);
}



