using GildedRose.App.ApiClient.Models;

namespace GildedRose.App;

public static class ItemsReportFormatter
{
    public static string Format(int day, IEnumerable<ItemModel> items)
    {
        var lines = new List<string>
        {
            $"-------- day {day} --------",
            "name, sellIn, quality"
        };

        lines.AddRange(items.Select(item => $"{item.Name}, {item.SellIn}, {item.Quality}"));
        lines.Add(string.Empty);

        return string.Join(Environment.NewLine, lines) + Environment.NewLine;
    }
}
