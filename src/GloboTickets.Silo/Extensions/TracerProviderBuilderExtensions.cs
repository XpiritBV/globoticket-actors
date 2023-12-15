using GloboTickets.Silo.OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace GloboTickets.Silo.Extensions;

public static partial class TracerProviderBuilderExtensions
{
    public static TracerProviderBuilder AddIf(
        this TracerProviderBuilder builder,
        bool condition,
        Func<TracerProviderBuilder, TracerProviderBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            builder = action(builder);
        }

        return builder;
    }
    public static IServiceCollection ConfigureAspNetCoreInstrumentation(this IServiceCollection services)
    {
        services.Configure<AspNetCoreInstrumentationOptions>(options =>
        {
            options.EnrichWithHttpRequest = EnrichWithHttpRequest;
            options.EnrichWithHttpResponse = EnrichWithHttpResponse;
            options.RecordException = true;
        });
        return services;
    }

    /// <summary>
    /// Enrich spans with additional request meta data.
    /// See https://github.com/open-telemetry/opentelemetry-specification/blob/master/specification/trace/semantic_conventions/http.md.
    /// </summary>
    private static void EnrichWithHttpRequest(Activity activity, HttpRequest request)
    {
        var context = request.HttpContext;
        activity.AddTag(OpenTelemetryAttributeName.Http.ClientIP, context.Connection.RemoteIpAddress);
        activity.AddTag(OpenTelemetryAttributeName.Http.RequestContentLength, request.ContentLength);
        activity.AddTag(OpenTelemetryAttributeName.Http.RequestContentType, request.ContentType);

        var user = context.User;
        if (user.Identity?.Name is not null)
        {
            activity.AddTag(OpenTelemetryAttributeName.EndUser.Id, user.Identity.Name);
            activity.AddTag(OpenTelemetryAttributeName.EndUser.Scope, string.Join(',', user.Claims.Select(x => x.Value)));
        }
    }

    /// <summary>
    /// Enrich spans with additional response meta data.
    /// See https://github.com/open-telemetry/opentelemetry-specification/blob/master/specification/trace/semantic_conventions/http.md.
    /// </summary>
    private static void EnrichWithHttpResponse(Activity activity, HttpResponse response)
    {
        activity.AddTag(OpenTelemetryAttributeName.Http.ResponseContentLength, response.ContentLength);
        activity.AddTag(OpenTelemetryAttributeName.Http.ResponseContentType, response.ContentType);
    }

    public static TracerProviderBuilder AddOrleansTracing(
        this TracerProviderBuilder builder,
        IWebHostEnvironment webHostEnvironment) =>
        builder
            .AddSource("Microsoft.Orleans.Runtime")
            .AddSource("Microsoft.Orleans.Application");

}