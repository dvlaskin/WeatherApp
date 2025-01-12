namespace WebApi.Utils;

public static class WmoCodesConverter
{
    public static string ToWmoCode(this int wmoCode)
    {
        return wmoCode switch
        {
            0 => "Clear sky",
            (>=1) and (<=3) => "Mainly clear, partly cloudy, and overcast",
            (>=45) and (<=48) => "Fog and depositing rime fog",
            (>=51) and (<=55) => "Drizzle: Light, moderate, and dense intensity",
            (>=56) and (<=57) => "Freezing Drizzle: Light and dense intensity",
            (>=61) and (<=65) => "Rain: Slight, moderate and heavy intensity",
            (>=66) and (<=67) => "Freezing Rain: Light and heavy intensity",
            (>=71) and (<=75) => "Snow fall: Slight, moderate, and heavy intensity",
            77 => "Snow grains",
            (>=80) and (<=82) => "Rain showers: Slight, moderate, and violent",
            (>=85) and (<=86) => "Snow showers slight and heavy",
            95 => "Thunderstorm: Slight or moderate",
            (>=96) and (<=99) => "Thunderstorm with slight and heavy hail",
            _ => string.Empty
        };
    }
}