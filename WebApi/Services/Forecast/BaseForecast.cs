using WebApi.Models;

namespace WebApi.Services.Forecast;

public interface IForecastService
{
    Task<IEnumerable<WeatherForecastData>> FetchDataAsync(string cityName, double latitude, double longitude);
}

public abstract class BaseForecastService : IForecastService
{
    private readonly string cacheKey = "forecast:{0}_{1}_{2}";
    private readonly ILogger<BaseForecastService> logger;
    private readonly ICacheService cacheService;
    private readonly int expiredTimeout = 60;

    
    protected BaseForecastService(ILogger<BaseForecastService> logger, ICacheService cacheService)
    {
        this.logger = logger;
        this.cacheService = cacheService;
    }
    
    
    public async Task<IEnumerable<WeatherForecastData>> FetchDataAsync(string cityName, double latitude, double longitude)
    {
        var cityCacheKey = string.Format(cacheKey, cityName, latitude, longitude);
        var cachedForecast = await cacheService.GetAsync<List<WeatherForecastData>>(cityCacheKey);

        if (cachedForecast is not null)
        {
            logger.LogInformation("Found cache forecasts for {City}", cityName);
            return cachedForecast;
        }
        
        cachedForecast = await RequestDataAsync(cityName, latitude, longitude);
        
        logger.LogInformation("Requested forecasts for {City}", cityName);
        await cacheService.SetAsync(cityCacheKey, cachedForecast, TimeSpan.FromMinutes(expiredTimeout));
        
        return cachedForecast;
    }
    
    protected abstract Task<List<WeatherForecastData>> RequestDataAsync(string cityName, double latitude, double longitude);
}