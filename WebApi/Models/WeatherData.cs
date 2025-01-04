namespace WebApi.Models;

public class WeatherData()
{
    public DateOnly Date { get; set; }
    public double TemperatureC { get; set; }
    public double FeelsLikeC { get; set; }
    public string? Summary { get; set; }
    public DateTime ForecastDate { get; set; } = DateTime.UtcNow;

    public double TemperatureF => 32 + (TemperatureC / 0.5556);
    public double FeelsLikeF => 32 + (FeelsLikeC / 0.5556);
}