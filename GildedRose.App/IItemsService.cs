using GildedRose.App.ApiClient.Models;

namespace GildedRose.App;

public interface IItemsService
{
    event Func<Task>? ItemsChanged;

    Task ConnectAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ItemModel>> GetItemsAsync(CancellationToken cancellationToken = default);
}
