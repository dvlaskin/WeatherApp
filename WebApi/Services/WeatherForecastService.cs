using StackExchange.Redis;
using WebApi.Models;
using WebApi.Models.OpenWeatherMap;
using WebApi.Services.Forecast;

namespace WebApi.Services;

public class WeatherForecastService
{
    private readonly ILogger<WeatherForecastService> logger;
    private readonly IForecastCollector forecastCollector;
    private readonly IGeoDataService geoDataService;

    public WeatherForecastService(
        ILogger<WeatherForecastService> logger, 
        IForecastCollector forecastCollector,
        IGeoDataService geoDataService
    )
    {
        this.logger = logger;
        this.forecastCollector = forecastCollector;
        this.geoDataService = geoDataService;
    }
    
    
    public async Task<IEnumerable<WeatherForecastData>> GetForecastAsync(string city)
    {
        logger.LogInformation("Getting weather forecast for city {City}", city);
        
        var forecasts = await forecastCollector.CollectForecastAsync(city);
        
        return forecasts.Values.SelectMany(s => s).ToArray();
    }

    public async Task<IEnumerable<CityCoordinate>> GetCoordinatesAsync(string city)
    {
        logger.LogInformation("Getting weather forecast for city {City}", city);
        
        return await geoDataService.GetCitiesCoordinateAsync(city);
    }
}