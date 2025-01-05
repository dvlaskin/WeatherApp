using WebApi.Services;
using WebApi.Services.CurrentWeather;
using WebApi.Services.Forecast;

namespace WebApi.IoC;

public static class ServicesControl
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, RedisService>();
        services.AddScoped<ICurrentWeatherService, OpenMeteoCurrentWeather>();
        services.AddScoped<ICurrentWeatherCollector, CurrentWeatherCollector>();
        services.AddScoped<IForecastService, OpenMeteoForecastService>();
        services.AddScoped<IForecastCollector, ForecastCollector>();
        services.AddScoped<IGeoDataService, GeoDataService>();
        services.AddScoped<WeatherService>();
        
        return services;
    }
}