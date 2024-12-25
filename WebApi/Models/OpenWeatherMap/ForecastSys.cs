using System.Text.Json.Serialization;

namespace WebApi.Models.OpenWeatherMap;

public class ForecastSys
{
    [JsonPropertyName("pod")]
    public string Pod { get; set; }
}