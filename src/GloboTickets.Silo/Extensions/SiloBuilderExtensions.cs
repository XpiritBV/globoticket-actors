﻿namespace GloboTickets.Silo.Extensions;

public static partial class SiloBuilderExtensions
{

    public static ISiloBuilder ConfigureDefaultSiloBuilder(this ISiloBuilder builder, HostBuilderContext context)
    {
        builder.ConfigureEndpoints(
                EndpointOptions.DEFAULT_SILO_PORT,
                EndpointOptions.DEFAULT_GATEWAY_PORT,
                listenOnAnyHostAddress: !context.HostingEnvironment.IsDevelopment())
            .AddActivityPropagation();

        return builder;
    }

    /// <summary>
    /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
    /// used to conditionally add to the silo builder.
    /// </summary>
    /// <param name="siloBuilder">The silo builder.</param>
    /// <param name="condition">If set to <c>true</c> the action is executed.</param>
    /// <param name="action">The action used to add to the silo builder.</param>
    /// <returns>The same silo builder.</returns>
    public static ISiloBuilder UseIf(
        this ISiloBuilder siloBuilder,
        bool condition,
        Func<ISiloBuilder, ISiloBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(siloBuilder);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            siloBuilder = action(siloBuilder);
        }

        return siloBuilder;
    }

    /// <summary>
    /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
    /// used to conditionally add to the silo builder.
    /// </summary>
    /// <param name="siloBuilder">The silo builder.</param>
    /// <param name="condition">If <c>true</c> is returned the action is executed.</param>
    /// <param name="action">The action used to add to the silo builder.</param>
    /// <returns>The same silo builder.</returns>
    public static ISiloBuilder UseIf(
        this ISiloBuilder siloBuilder,
        Func<ISiloBuilder, bool> condition,
        Func<ISiloBuilder, ISiloBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(siloBuilder);
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(action);

        if (condition(siloBuilder))
        {
            siloBuilder = action(siloBuilder);
        }

        return siloBuilder;
    }
}