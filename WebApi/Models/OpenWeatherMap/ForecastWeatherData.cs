using System.Text.Json.Serialization;

namespace WebApi.Models.OpenWeatherMap;

public class ForecastWeatherData
{
    [JsonPropertyName("cod")]
    public string Cod { get; set; }

    [JsonPropertyName("message")]
    public int Message { get; set; }

    [JsonPropertyName("cnt")]
    public int Cnt { get; set; }

    [JsonPropertyName("list")]
    public List<ResultList> ResultsList { get; set; } = new List<ResultList>();

    [JsonPropertyName("city")]
    public City City { get; set; }
}