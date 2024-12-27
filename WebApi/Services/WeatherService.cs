using WebApi.Models;
using WebApi.Services.CurrentWeather;
using WebApi.Services.Forecast;
using WebApi.Utils;

namespace WebApi.Services;

public class WeatherService
{
    private readonly ILogger<WeatherService> logger;
    private readonly ICurrentWeatherCollector currentWeatherCollector;
    private readonly IForecastCollector forecastCollector;
    private readonly ICacheService cacheService;

    private readonly string currentWeatherCacheKey = "currentWeather:{0}_{1}_{2}";
    private readonly string forecastWeatherCacheKey = "forecastWeather:{0}_{1}_{2}";

    private readonly int expiredTimeout = 60;

    
    public WeatherService(
        ILogger<WeatherService> logger,
        ICurrentWeatherCollector currentWeatherCollector,
        IForecastCollector forecastCollector,
        ICacheService cacheService
    )
    {
        this.logger = logger;
        this.currentWeatherCollector = currentWeatherCollector;
        this.forecastCollector = forecastCollector;
        this.cacheService = cacheService;
    }
    
    
    public async Task<WeatherData> GetCurrentWeatherAsync(string cityName, double latitude, double longitude)
    {
        logger.LogInformation("Getting current weather for city {City}", cityName);
        
        var cityCacheKey = string.Format(
            currentWeatherCacheKey, cityName.KeyNormalization(), latitude, longitude
        );
        var cachedWeather = await cacheService.GetAsync<WeatherData>(cityCacheKey);
        
        if (cachedWeather is not null)
        {
            logger.LogInformation("Found cache forecasts for {City}", cityName);
            return cachedWeather;
        }
        
        var weatherData = await currentWeatherCollector
            .CollectForecastAsync(cityName, latitude, longitude);
        
        // get avg weather values
        var currentWeather = new WeatherData
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            ForecastDate = DateTime.UtcNow,
            Summary = $"Current weather in {cityName}",
            TemperatureC = weatherData.Values.Average(x => x.TemperatureC)
        };
        
        await cacheService.SetAsync(cityCacheKey, currentWeather, TimeSpan.FromMinutes(expiredTimeout));
        
        return currentWeather;
    }
    
    public async Task<IEnumerable<WeatherData>> GetForecastAsync(string cityName, double latitude, double longitude)
    {
        logger.LogInformation("Getting weather forecast for city {City}", cityName);
        
        var cityCacheKey = string.Format(
            forecastWeatherCacheKey, cityName.KeyNormalization(), latitude, longitude
        );
        var cachedWeather = await cacheService.GetAsync<List<WeatherData>>(cityCacheKey);
        
        if (cachedWeather is not null)
        {
            logger.LogInformation("Found cache forecasts for {City}", cityName);
            return cachedWeather;
        }

        Dictionary<string, IEnumerable<WeatherData>> forecasts = await forecastCollector
            .CollectForecastAsync(cityName, latitude, longitude);


        // calculate avg weather values group by date
        List<WeatherData> forecastWeather = forecasts
            .Values
            .SelectMany(x => x)
            .GroupBy(x => x.Date)
            .Select(x => new WeatherData
            {
                Date = x.Key,
                ForecastDate = DateTime.UtcNow,
                Summary = $"Weather forecast for {cityName}",
                TemperatureC = x.Average(x => x.TemperatureC)
            })
            .ToList();
        
        await cacheService.SetAsync(cityCacheKey, forecastWeather, TimeSpan.FromMinutes(expiredTimeout));
        
        return forecastWeather;
    }
    
}