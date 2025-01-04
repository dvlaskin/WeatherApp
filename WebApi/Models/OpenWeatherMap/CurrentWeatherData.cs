using System.Text.Json.Serialization;

namespace WebApi.Models.OpenWeatherMap;

public class CurrentWeatherData
{
    [JsonPropertyName("coord")]
    public Coord? Coord { get; set; }
    
    [JsonPropertyName("weather")]
    public List<Weather> Weather { get; set; } = new List<Weather>();
    
    [JsonPropertyName("base")]
    public string? Base { get; set; }
    
    [JsonPropertyName("main")]
    public CurrentWeatherMain? Main { get; set; }
    
    [JsonPropertyName("visibility")]
    public int? Visibility { get; set; }
    
    [JsonPropertyName("wind")]
    public Wind? Wind { get; set; }
    
    [JsonPropertyName("rain")]
    public CurrentRain? Rain { get; set; }
    
    [JsonPropertyName("clouds")]
    public Clouds? Clouds { get; set; }
    
    [JsonPropertyName("dt")]
    public int? Dt { get; set; }
    
    [JsonPropertyName("sys")]
    public CurrentWeatherSys? Sys { get; set; }
    
    [JsonPropertyName("timezone")]
    public int? Timezone { get; set; }
    
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("cod")]
    public int? Cod { get; set; }
}

public class CurrentWeatherMain
{
    [JsonPropertyName("temp")]
    public double? Temp { get; set; }
    
    [JsonPropertyName("feels_like")]
    public double? FeelsLike { get; set; }
    
    [JsonPropertyName("temp_min")]
    public double? TempMin { get; set; }
    
    [JsonPropertyName("temp_max")]
    public double? TempMax { get; set; }
    
    [JsonPropertyName("pressure")]
    public int? Pressure { get; set; }
    
    [JsonPropertyName("humidity")]
    public int? Humidity { get; set; }
    
    [JsonPropertyName("sea_level")]
    public int? SeaLevel { get; set; }
    
    [JsonPropertyName("grnd_level")]
    public int? GrndLevel { get; set; }
}

public class CurrentRain
{
    [JsonPropertyName("1h")]
    public double? _1h { get; set; }
}

public class CurrentWeatherSys
{
    [JsonPropertyName("type")]
    public int? Type { get; set; }
    
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    
    [JsonPropertyName("sunrise")]
    public int? Sunrise { get; set; }
    
    [JsonPropertyName("sunset")]
    public int? Sunset { get; set; }
}
