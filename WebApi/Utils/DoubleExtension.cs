namespace WebApi.Utils;

public static class DoubleExtension
{
    public static string ToStringWithDot(this double value)
    {
        return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}