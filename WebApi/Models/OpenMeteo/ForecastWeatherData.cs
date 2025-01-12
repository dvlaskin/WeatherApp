using System.Text.Json.Serialization;

namespace WebApi.Models.OpenMeteo;

public class ForecastWeatherData
{
    [JsonPropertyName("daily")]
    public DailyForecast? DailyForecast { get; set; }
}

public class DailyForecast
{
    [JsonPropertyName("time")] 
    public List<string> WeatherDates { get; set; } = [];
    
    [JsonPropertyName("weather_code")]
    public List<int> WeatherCodes { get; set; } = [];
    
    [JsonPropertyName("temperature_2m_max")]
    public List<double> TempMax { get; set; } = [];
    
    [JsonPropertyName("temperature_2m_min")]
    public List<double> TempMin { get; set; } = [];
    
    [JsonPropertyName("apparent_temperature_max")]
    public List<double> FeelsLikeMax { get; set; } = [];
    
    [JsonPropertyName("apparent_temperature_min")]
    public List<double> FeelsLikeMin { get; set; } = [];
}