using Microsoft.AspNetCore.SignalR;

namespace Api.Items.Hubs;

public class ItemsSignalRHub : Hub<IItemsClient>;