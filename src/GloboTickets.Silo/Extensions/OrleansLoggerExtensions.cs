namespace GloboTickets.Silo.Extensions;

/// <summary>
/// <see cref="ILogger"/> extension methods. Helps log messages using strongly typing and source generators.
/// </summary>
public static partial class OrleansLoggerExtensions
{
    [LoggerMessage(
        EventId = 5003,
        Level = LogLevel.Error,
        Message = "Failed cluster status health check.")]
    public static partial void FailedClusterStatusHealthCheck(this ILogger logger, Exception exception);

    [LoggerMessage(
        EventId = 5004,
        Level = LogLevel.Error,
        Message = "Failed local health check.")]
    public static partial void FailedLocalHealthCheck(this ILogger logger, Exception exception);

    [LoggerMessage(
        EventId = 5005,
        Level = LogLevel.Error,
        Message = "Failed storage health check.")]
    public static partial void FailedStorageHealthCheck(this ILogger logger, Exception exception);
}