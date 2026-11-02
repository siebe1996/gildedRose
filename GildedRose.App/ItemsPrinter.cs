namespace GildedRose.App;

public sealed class ItemsPrinter(IItemsService itemsService, TextWriter writer)
{
    private int _day;

    public void Subscribe() => itemsService.ItemsChanged += PrintAsync;

    public async Task PrintAsync()
    {
        try
        {
            var items = await itemsService.GetItemsAsync();
            await writer.WriteAsync(ItemsReportFormatter.Format(_day, items));
            _day++;
        }
        catch (Exception ex)
        {
            await writer.WriteLineAsync("Could not load items: " + ex.Message);
        }
    }
}
