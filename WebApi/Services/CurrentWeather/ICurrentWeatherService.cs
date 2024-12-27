using WebApi.Models;

namespace WebApi.Services.Forecast;

public interface ICurrentWeatherService
{
    Task<WeatherData> FetchDataAsync(string cityName, double latitude, double longitude);
}