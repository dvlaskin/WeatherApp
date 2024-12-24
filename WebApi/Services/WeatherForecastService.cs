using StackExchange.Redis;
using WebApi.Models;
using WebApi.Services.Forecast;

namespace WebApi.Services;

public class WeatherForecastService
{
    private readonly ILogger<WeatherForecastService> logger;
    private readonly IForecastCollector forecastCollector;

    public WeatherForecastService(ILogger<WeatherForecastService> logger, IForecastCollector forecastCollector)
    {
        this.logger = logger;
        this.forecastCollector = forecastCollector;
    }
    
    
    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(string city)
    {
        logger.LogInformation("Getting weather forecast for city {City}", city);
        
        var forecasts = await forecastCollector.CollectForecastAsync(city);
        
        return forecasts.Values.SelectMany(s => s).ToArray();
    }
}