using System.Text.Json;
using WebApi.Models;
using WebApi.Models.OpenMeteo;
using WebApi.Utils;

namespace WebApi.Services.Forecast;

public class OpenMeteoForecast : IForecastService
{
    private readonly IHttpClientFactory httpClientFactory;

    public OpenMeteoForecast(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<WeatherData>> FetchDataAsync(string cityName, double latitude, double longitude)
    {
        var result = new List<WeatherData>();
        var latitudeStr = latitude.ToStringWithDot();
        var longitudeStr = longitude.ToStringWithDot();
        var httpClient = httpClientFactory.CreateClient(AppConstants.OpenMeteoHttpClient);
        var urlString = $"/v1/forecast?latitude={latitudeStr}&longitude={longitudeStr}&daily=weather_code,temperature_2m_max,temperature_2m_min,apparent_temperature_max,apparent_temperature_min&timezone=GMT&forecast_days=6";
        var response = await httpClient.GetAsync(urlString);

        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        var weatherForecast = string.IsNullOrEmpty(responseString) 
            ? new ForecastWeatherData()
            : JsonSerializer.Deserialize<ForecastWeatherData>(responseString)!;
        
        for (var i = 1; i < weatherForecast.DailyForecast?.WeatherDates.Count; i++)
        {
            if (weatherForecast.DailyForecast?.WeatherDates is null)
            {
                result.Clear();
                break;
            }
            
            var forecastDate = DateOnly.Parse(weatherForecast.DailyForecast?.WeatherDates[i]!);
            result.Add(
                new()
                {
                    Date = forecastDate,
                    ForecastDate = DateTime.UtcNow,
                    TemperatureC = 
                    (
                        weatherForecast.DailyForecast?.TempMax[i] ?? 0 
                        + weatherForecast.DailyForecast?.TempMin[i] ?? 0
                    ) / 2.0d,
                    FeelsLikeC = 
                    (
                        weatherForecast.DailyForecast?.FeelsLikeMax[i] ?? 0 
                        + weatherForecast.DailyForecast?.FeelsLikeMin[i] ?? 0
                    ) / 2.0d,
                    Summary = weatherForecast.DailyForecast?.WeatherCodes[i].ToWmoCode()
                }
            );
        }
        
        return result;
    }
}