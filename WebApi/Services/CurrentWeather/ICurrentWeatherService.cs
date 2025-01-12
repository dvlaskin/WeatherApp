using WebApi.Models;

namespace WebApi.Services.CurrentWeather;

public interface ICurrentWeatherService
{
    Task<WeatherData> FetchDataAsync(string cityName, double latitude, double longitude);
}