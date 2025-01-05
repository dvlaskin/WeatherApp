using System.Reflection;

namespace WebApi.IoC;

public static class HttpClientControl
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenWeatherMapHttpClient(configuration);
        
        return services;
    }

    private static IServiceCollection AddOpenWeatherMapHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient(
            AppConstants.OpenWeatherMapHttpClient,
            client =>
            {
                client.BaseAddress = new Uri(
                    configuration.GetSection("OpenWeatherMap:BaseUrl").Value ?? string.Empty
                );
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }
        );
        
        return services;  
    }
}