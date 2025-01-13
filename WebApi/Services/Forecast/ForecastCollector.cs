using WebApi.Models;

namespace WebApi.Services.Forecast;

public interface IForecastCollector
{
    Task<Dictionary<string, IReadOnlyList<WeatherData>>> CollectForecastAsync(string cityName, double latitude, double longitude);
}

public class ForecastCollector : IForecastCollector
{
    private readonly IEnumerable<IForecastService> forecastServices;
    
    public ForecastCollector(IEnumerable<IForecastService> forecastServices)
    {
        this.forecastServices = forecastServices;
    }
    
    public async Task<Dictionary<string, IReadOnlyList<WeatherData>>> CollectForecastAsync(string cityName, double latitude, double longitude)
    {
        var result = new Dictionary<string, IReadOnlyList<WeatherData>>();
        var forecastTasks = new Dictionary<string, Task<IReadOnlyList<WeatherData>>>();
        foreach (var forecast in forecastServices)
        {
            var taskItem = forecast.FetchDataAsync(cityName, latitude, longitude);
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