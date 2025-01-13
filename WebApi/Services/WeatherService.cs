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

    private const string CurrentWeatherCacheKey = "currentWeather:{0}_{1}_{2}";
    private const string ForecastWeatherCacheKey = "forecastWeather:{0}_{1}_{2}";

    private readonly int expirationTimeout;

    
    public WeatherService(
        IConfiguration config,
        ILogger<WeatherService> logger,
        ICurrentWeatherCollector currentWeatherCollector,
        IForecastCollector forecastCollector,
        ICacheService cacheService
    )
    {
        this.expirationTimeout = config.GetValue<int>("WeatherCache:ExpirationTimeoutMinutes");
        
        this.logger = logger;
        this.currentWeatherCollector = currentWeatherCollector;
        this.forecastCollector = forecastCollector;
        this.cacheService = cacheService;
    }
    
    
    public async Task<WeatherData> GetCurrentWeatherAsync(string cityName, double latitude, double longitude)
    {
        logger.LogInformation("Getting current weather for city {City}", cityName);
        
        var cityCacheKey = string.Format(
            CurrentWeatherCacheKey, cityName.KeyNormalization(), latitude, longitude
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
            Summary = $"Current weather in {cityName}: {weatherData.Values.First().Summary}",
            TemperatureC = weatherData.Values.Average(x => x.TemperatureC),
            FeelsLikeC = weatherData.Values.Average(x => x.FeelsLikeC),
        };
        
        await cacheService.SetAsync(cityCacheKey, currentWeather, TimeSpan.FromMinutes(expirationTimeout));
        
        return currentWeather;
    }
    
    public async Task<IReadOnlyList<WeatherData>> GetForecastAsync(string cityName, double latitude, double longitude)
    {
        logger.LogInformation("Getting weather forecast for city {City}", cityName);
        
        var cityCacheKey = string.Format(
            ForecastWeatherCacheKey, cityName.KeyNormalization(), latitude, longitude
        );
        var cachedWeather = await cacheService.GetAsync<List<WeatherData>>(cityCacheKey);
        
        if (cachedWeather is not null)
        {
            logger.LogInformation("Found cache forecasts for {City}", cityName);
            return cachedWeather;
        }

        Dictionary<string, IReadOnlyList<WeatherData>> forecasts = await forecastCollector
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
                Summary = $"Weather forecast for {cityName}: {x.ToList().GetRandom().Summary}",
                TemperatureC = x.Average(avg => avg.TemperatureC),
                FeelsLikeC = x.Average(avg => avg.FeelsLikeC)
            })
            .ToList();
        
        await cacheService.SetAsync(cityCacheKey, forecastWeather, TimeSpan.FromMinutes(expirationTimeout));
        
        return forecastWeather;
    }
    
}