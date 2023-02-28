using Microsoft.Extensions.Logging;

namespace Costasdev.Geminet;

/**
 * Utility to create loggers with a common configuration.
 */
public static class Loggers
{
    /**
     * Creates a logger with a common configuration and the specified name.
     */
    public static ILogger CreateLogger(string name)
    {
        return LoggerFactory.Create(conf => conf
                .SetMinimumLevel(LogLevel.Debug)
                .AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                })
            )
            .CreateLogger(name);
    }
}