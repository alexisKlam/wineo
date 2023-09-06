namespace Microsoft.Extensions.DependencyInjection.Helpers;

public static class RandomHelper
{
    private static readonly Random random = new Random();

    public static double GetNextDouble()
    {
        return random.NextDouble();
    }
    public static decimal GetNextPrice()
    {
        return (decimal)GetNextDouble() *100;
    }

    public static T GetNextRandomList<T>(this IList<T> items)
    {
        int randomIndex = random.Next(0, items.Count);
        return items[randomIndex];
    }

    public static IList<T> GetNextXRandomItem<T>(this IList<T> items, int total)
    {
        return items.OrderBy(_ => random.Next()).Take(total).ToList();

    }
}