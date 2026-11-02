using Microsoft.AspNetCore.SignalR.Client;
using SignalR.Client.DependencyInjection;

namespace SignalR.Client.Clients.Items;

public sealed class ItemsHubClient : BaseClient, IItemsNotificationClient
{
    public ItemsHubClient(ApiOptions options) : base(options.BaseUrl + "/hubs/items")
    {
        // Registered once. Must match the method name on IItemsClient in the API.
        Connection.On("ItemsUpdated", RaiseItemsUpdatedAsync);
    }

    public event Func<Task>? ItemsUpdated;

    private async Task RaiseItemsUpdatedAsync()
    {
        var handlers = ItemsUpdated;
        if (handlers is null)
        {
            return;
        }

        foreach (var handler in handlers.GetInvocationList().Cast<Func<Task>>())
        {
            await handler();
        }
    }
}
