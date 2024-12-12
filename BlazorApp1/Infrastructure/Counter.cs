namespace BlazorApp1.Infrastructure;

public static class Counter
{
    public static int FirstFreeId { get; private set; }
    public static string Next()
    {
        Console.WriteLine(FirstFreeId);
        return (++FirstFreeId).ToString();
    }

    public static void UpdateLast(int lastId) => FirstFreeId = lastId;
}