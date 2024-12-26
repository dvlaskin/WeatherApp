using System.Text.Json;
using WebApi.Models;
using WebApi.Models.OpenWeatherMap;

namespace WebApi.Services.Forecast;

public class OpenMeteoForecastService : BaseForecastService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string apiKey;

    public OpenMeteoForecastService(
        ILogger<OpenMeteoForecastService> logger,
        ICacheService cacheService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    ) : base(logger, cacheService)
    {
        this.httpClientFactory = httpClientFactory;
        this.apiKey = configuration["ApiKeys:OpenWeatherMapApiKey"] ?? string.Empty;
    }

    protected override async Task<List<WeatherForecastData>> RequestDataAsync(string cityName, double latitude, double longitude)
    {
        var result = new List<WeatherForecastData>();
        var httpClient = httpClientFactory.CreateClient(AppConstants.OpenWeatherMapHttpClient);
        var urlString = $"/data/2.5/weather?lat={latitude}&lon={longitude}&exclude=minutely,hourly&units=metric&appid={apiKey}";
        var response = await httpClient.GetAsync(urlString);

        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        var currentWeather = string.IsNullOrEmpty(responseString) 
            ? new CurrentWeatherData()
            : JsonSerializer.Deserialize<CurrentWeatherData>(responseString);
        
        result.Add(
            new()
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                ForecastDate = DateTime.UtcNow,
                TemperatureC = currentWeather?.Main.Temp ?? 0,
            }
        );
        
        return result;
    }
}