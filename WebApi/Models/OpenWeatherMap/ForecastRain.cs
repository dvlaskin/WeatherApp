using System.Text.Json.Serialization;

namespace WebApi.Models.OpenWeatherMap;

public class ForecastRain
{
    [JsonPropertyName("3h")]
    public double _3h { get; set; }
}