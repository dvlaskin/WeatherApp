using WebApi.Models;

namespace WebApi.Services.Forecast;

public interface IForecastService
{
    Task<IEnumerable<WeatherData>> FetchDataAsync(string cityName, double latitude, double longitude);
}