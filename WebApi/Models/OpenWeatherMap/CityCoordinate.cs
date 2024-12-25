using System.Text.Json.Serialization;

namespace WebApi.Models.OpenWeatherMap;

public class CityCoordinate
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }
}
