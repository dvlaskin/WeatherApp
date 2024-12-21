using StackExchange.Redis;
using WebApi.Models;

namespace WebApi.Services;

public class WeatherForecastService
{
    private readonly ILogger<WeatherForecastService> logger;
    private readonly IConnectionMultiplexer cache;
    private readonly string[] summaries = 
        ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    
    public WeatherForecastService(ILogger<WeatherForecastService> logger, IConnectionMultiplexer cache)
    {
        this.logger = logger;
        this.cache = cache;
    }
    
    
    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(string city)
    {
        logger.LogInformation($"Getting weather forecast for city {city}");
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        
        return await Task.FromResult(forecast);
    }
}