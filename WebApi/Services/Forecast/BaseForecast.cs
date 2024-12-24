using WebApi.Models;

namespace WebApi.Services.Forecast;

public interface IForecastService
{
    Task<IEnumerable<WeatherForecast>> FetchDataAsync(string city);
}

public abstract class BaseForecastService : IForecastService
{
    private readonly string cacheKey = "{0}-forecast";
    private readonly ILogger<BaseForecastService> logger;
    private readonly ICacheService cacheService;
    private readonly int expiredTimeout = 60;

    
    protected BaseForecastService(ILogger<BaseForecastService> logger, ICacheService cacheService)
    {
        this.logger = logger;
        this.cacheService = cacheService;
    }
    
    
    public async Task<IEnumerable<WeatherForecast>> FetchDataAsync(string city)
    {
        var cityCacheKey = string.Format(cacheKey, city);
        var cachedForecast = await cacheService.GetAsync<List<WeatherForecast>>(cityCacheKey);

        if (cachedForecast is not null)
        {
            logger.LogInformation("Found cache forecasts for {City}", city);
            return cachedForecast;
        }
        
        cachedForecast = await RequestDataAsync(city);
        
        logger.LogInformation("Requested forecasts for {City}", city);
        await cacheService.SetAsync(cityCacheKey, cachedForecast, TimeSpan.FromMinutes(expiredTimeout));
        
        return cachedForecast;
    }
    
    protected abstract Task<List<WeatherForecast>> RequestDataAsync(string city);
}