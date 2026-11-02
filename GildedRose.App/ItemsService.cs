using GildedRose.App.ApiClient;
using GildedRose.App.ApiClient.Models;
using SignalR.Client;
using SignalR.Client.Clients.Items;

namespace GildedRose.App;

public sealed class ItemsService(ISignalRClientFactory clientFactory, GildedRoseApiClient apiClient) : IItemsService
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IReadOnlyList<ItemModel>? _cache;

    public event Func<Task>? ItemsChanged;

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        var client = clientFactory.GetClient<IItemsNotificationClient>();
        client.ItemsUpdated += OnItemsUpdatedAsync;

        await client.ConnectWithRetryAsync(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), cancellationToken);
    }

    public async Task<IReadOnlyList<ItemModel>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _cache ??= await apiClient.Api.Items.GetAsync(cancellationToken: cancellationToken) ?? [];
            return _cache;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task OnItemsUpdatedAsync()
    {
        await _lock.WaitAsync();
        try
        {
            _cache = null;
        }
        finally
        {
            _lock.Release();
        }

        if (ItemsChanged is not { } handlers)
        {
            return;
        }

        foreach (var handler in handlers.GetInvocationList().Cast<Func<Task>>())
        {
            await handler();
        }
    }
}
