using System.Text.Json.Serialization;

namespace WebApi.Models.OpenMeteo;

public class CurrentWeatherData
{
    [JsonPropertyName("current")]
    public CurrentWeatherMain? CurrentWeather { get; set; }
}

public class CurrentWeatherMain
{
    [JsonPropertyName("time")]
    public string? WeatherDateTime { get; set; }
    
    [JsonPropertyName("temperature_2m")]
    public double? Temp { get; set; }
    
    [JsonPropertyName("apparent_temperature")]
    public double? FeelsLike { get; set; }
    
    [JsonPropertyName("weather_code")]
    public int? WeatherCode { get; set; }
}