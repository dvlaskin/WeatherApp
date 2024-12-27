using System.Text.Json;
using WebApi.Models;
using WebApi.Models.OpenWeatherMap;

namespace WebApi.Services.Forecast;

public class OpenMeteoForecastService : IForecastService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string apiKey;

    public OpenMeteoForecastService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    )
    {
        this.httpClientFactory = httpClientFactory;
        this.apiKey = configuration["ApiKeys:OpenWeatherMapApiKey"] ?? string.Empty;
    }

    public async Task<IEnumerable<WeatherData>> FetchDataAsync(string cityName, double latitude, double longitude)
    {
        var result = new List<WeatherData>();
        var httpClient = httpClientFactory.CreateClient(AppConstants.OpenWeatherMapHttpClient);
        var urlString = $"/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&appid={apiKey}";
        var response = await httpClient.GetAsync(urlString);

        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        var weatherForecast = string.IsNullOrEmpty(responseString) 
            ? new ForecastWeatherData()
            : JsonSerializer.Deserialize<ForecastWeatherData>(responseString)!;
        

        foreach (var item in weatherForecast.ResultsList)
        {
            var forecastDate = DateTimeOffset.FromUnixTimeSeconds(item.Dt).DateTime;
            
            if (forecastDate.Hour > 0)
                continue;
            
            result.Add(
                new()
                {
                    Date = DateOnly.FromDateTime(forecastDate),
                    ForecastDate = DateTime.UtcNow,
                    TemperatureC = item.Main.Temp
                }
            );
        }
        
        return result;
    }
}