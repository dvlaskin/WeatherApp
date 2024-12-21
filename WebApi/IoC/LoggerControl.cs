namespace WebApi.IoC;

public static class LoggerControl
{
    public static ILoggingBuilder AddLogger(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder    
            .ClearProviders()
            .AddConsole()
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("StackExchange.Redis", LogLevel.Warning);
        
        return loggingBuilder;
    }

    public static ILogger CreateStartupLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });

        return loggerFactory.CreateLogger("StartupLogger");
    }
}