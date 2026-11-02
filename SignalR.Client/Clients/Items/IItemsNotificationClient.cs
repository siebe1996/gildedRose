using SignalR.Client.Clients;

namespace SignalR.Client.Clients.Items;

public interface IItemsNotificationClient : IBaseClient
{
    /// <summary>
    ///     Raised when the hub reports that the items have changed.
    /// </summary>
    event Func<Task>? ItemsUpdated;
}
