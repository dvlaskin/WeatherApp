namespace WebApi.Utils;

public static class StringExtension
{
    public static string KeyNormalization(this string keyValue)
    {
        return keyValue.Replace(" ", "").ToLower();
    }
}