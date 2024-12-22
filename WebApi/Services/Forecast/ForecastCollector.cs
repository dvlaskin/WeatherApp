using WebApi.Models;

namespace WebApi.Services.Forecast;


public interface IForecastCollector
{
    Task<Dictionary<string, IEnumerable<WeatherForecast>>> CollectForecastAsync(string city);
}


public class ForecastCollector : IForecastCollector
{
    private readonly ILogger<ForecastCollector> logger;
    private readonly IEnumerable<IForecastService> forecastServices;

    
    public ForecastCollector(ILogger<ForecastCollector> logger, IEnumerable<IForecastService> forecastServices)
    {
        this.logger = logger;
        this.forecastServices = forecastServices;
    }


    public async Task<Dictionary<string, IEnumerable<WeatherForecast>>> CollectForecastAsync(string city)
    {
        var result = new Dictionary<string, IEnumerable<WeatherForecast>>();
        var forecastTasks = new Dictionary<string, Task<IEnumerable<WeatherForecast>>>();
        foreach (var forecast in forecastServices)
        {
            var taskItem = forecast.FetchDataAsync(city);
            forecastTasks.Add(forecast.GetType().Name, taskItem);
        }
        
        await Task.WhenAll(forecastTasks.Values);
        foreach (var res in forecastTasks)
        {
            var forecastValue = await res.Value;
            result.Add(res.Key, forecastValue);
        }
        
        return result;
    }
}