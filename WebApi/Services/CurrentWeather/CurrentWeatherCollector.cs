using WebApi.Models;
using WebApi.Services.Forecast;

namespace WebApi.Services.CurrentWeather;

public interface ICurrentWeatherCollector
{
    Task<Dictionary<string, WeatherData>> CollectForecastAsync(string cityName, double latitude, double longitude);
}


public class CurrentWeatherCollector : ICurrentWeatherCollector
{
    private readonly IEnumerable<ICurrentWeatherService> currentWeatherServices;

    public CurrentWeatherCollector(IEnumerable<ICurrentWeatherService> currentWeatherServices)
    {
        this.currentWeatherServices = currentWeatherServices;
    }
    
    
    public async Task<Dictionary<string, WeatherData>> CollectForecastAsync(string cityName, double latitude, double longitude)
    {
        var result = new Dictionary<string, WeatherData>();
        var weatherTasks = new Dictionary<string, Task<WeatherData>>();
        foreach (var currentWeatherService in currentWeatherServices)
        {
            var taskItem = currentWeatherService.FetchDataAsync(cityName, latitude, longitude);
            weatherTasks.Add(currentWeatherService.GetType().Name, taskItem);
        }
        
        await Task.WhenAll(weatherTasks.Values);
        foreach (var res in weatherTasks)
        {
            var forecastValue = await res.Value;
            result.Add(res.Key, forecastValue);
        }
        
        return result;
    }
}