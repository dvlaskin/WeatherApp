using System.Text.Json;
using WebApi.Models;
using WebApi.Models.OpenWeatherMap;
using WebApi.Services.Forecast;

namespace WebApi.Services.CurrentWeather;

public class OpenWeatherMapCurrentWeather : ICurrentWeatherService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string apiKey;

    public OpenWeatherMapCurrentWeather(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClientFactory = httpClientFactory;
        this.apiKey = configuration["ApiKeys:OpenWeatherMapApiKey"] ?? string.Empty;
    }
    
    public async Task<WeatherData> FetchDataAsync(string cityName, double latitude, double longitude)
    {
        var httpClient = httpClientFactory.CreateClient(AppConstants.OpenWeatherMapHttpClient);
        var urlString = $"/data/2.5/weather?lat={latitude}&lon={longitude}&exclude=minutely,hourly&units=metric&appid={apiKey}";
        var response = await httpClient.GetAsync(urlString);

        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        var currentWeather = string.IsNullOrEmpty(responseString) 
            ? new CurrentWeatherData()
            : JsonSerializer.Deserialize<CurrentWeatherData>(responseString);
        
       var result = new WeatherData
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            ForecastDate = DateTime.UtcNow,
            TemperatureC = currentWeather?.Main?.Temp ?? 0,
            FeelsLikeC = currentWeather?.Main?.FeelsLike ?? 0,
            Summary = currentWeather?.Weather.Count > 0
                ? $"{currentWeather.Weather.First().Main} - {currentWeather.Weather.First().Description}"
                : string.Empty,
        };
        
        return result;
    }
}