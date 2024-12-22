using WebApi.Models;

namespace WebApi.Services.Forecast;

public class OpenMeteoForecastService : BaseForecastService
{
    public OpenMeteoForecastService(ILogger<OpenMeteoForecastService> logger, ICacheService cacheService) 
        : base(logger, cacheService)
    {
    }

    protected override async Task<List<WeatherForecast>> RequestDataAsync(string city)
    {
        var forecasts = new List<WeatherForecast>
        {
            new()
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                TemperatureC = 32,
                Summary = "test 1",
                ForecastDate = DateTime.UtcNow
            },
            new()
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TemperatureC = 27,
                Summary = "test 2",
                ForecastDate = DateTime.UtcNow
            },
            new()
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                TemperatureC = 24,
                Summary = "test 3",
                ForecastDate = DateTime.UtcNow
            },
        };
        
        return await Task.FromResult(forecasts);
    }
}