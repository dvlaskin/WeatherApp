using WebApi.Models;

namespace WebApi.Services.Forecast;

public interface IForecastService
{
    Task<IReadOnlyList<WeatherData>> FetchDataAsync(string cityName, double latitude, double longitude);
}