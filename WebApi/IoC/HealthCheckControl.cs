namespace WebApi.IoC;

public static class HealthCheckControl
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection serviceProvider)
    {
        serviceProvider
            .AddHealthChecks();
        
        return serviceProvider;
    }
}