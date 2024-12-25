using System.Text.Json.Serialization;

namespace WebApi.Models.OpenWeatherMap;

public class ResultList
{
    [JsonPropertyName("dt")]
    public int Dt { get; set; }

    [JsonPropertyName("main")]
    public ForecastMain Main { get; set; }

    [JsonPropertyName("weather")]
    public List<Weather> Weather { get; } = new List<Weather>();

    [JsonPropertyName("clouds")]
    public Clouds Clouds { get; set; }

    [JsonPropertyName("wind")]
    public Wind Wind { get; set; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    [JsonPropertyName("pop")]
    public double Pop { get; set; }

    [JsonPropertyName("rain")]
    public ForecastRain Rain { get; set; }

    [JsonPropertyName("sys")]
    public ForecastSys Sys { get; set; }

    [JsonPropertyName("dt_txt")]
    public string DtTxt { get; set; }
}