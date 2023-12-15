namespace GloboTickets.Silo.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder AddIf(
        this IApplicationBuilder builder,
        bool condition,
        Func<IApplicationBuilder, IApplicationBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(action);

        if (condition)
        {
            builder = action(builder);
        }

        return builder;
    }
}