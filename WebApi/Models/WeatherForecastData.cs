namespace WebApi.Models;

public class WeatherForecastData()
{
    public DateOnly Date { get; set; }
    public double TemperatureC { get; set; }
    public string? Summary { get; set; }
    public DateTime ForecastDate { get; set; } = DateTime.UtcNow;

    public double TemperatureF => 32 + (TemperatureC / 0.5556);
}