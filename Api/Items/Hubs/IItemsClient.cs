namespace Api.Items.Hubs;

/// <summary>
///     The methods the server can call on connected clients.
/// </summary>
public interface IItemsClient
{
    Task ItemsUpdated();
}