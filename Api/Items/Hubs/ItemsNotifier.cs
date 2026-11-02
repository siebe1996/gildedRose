using Microsoft.AspNetCore.SignalR;

namespace Api.Items.Hubs;

/// <remarks>
///     The idea is to inject this class/interface into controllers that need to send something through the hub.
///     Instead of having logic everywhere on how to send something or decision logic to determine the clients to send to.
///     See: https://github.com/aspnet/SignalR/issues/182
/// </remarks>
internal sealed class ItemsNotifier(IHubContext<ItemsSignalRHub, IItemsClient> hubContext) : IItemsNotifier
{
    public Task NotifyItemsUpdatedAsync(CancellationToken cancellationToken = default)
    {
        return hubContext.Clients.All.ItemsUpdated();
    }
}