namespace WebApi.Utils;

public static class ListExtension
{
    public static T GetRandom<T>(this List<T> list)
    {
        return list[new Random().Next(list.Count)];
    }
}