using SignalR.Client.Clients;

namespace SignalR.Client;

public interface ISignalRClientFactory
{
    TClient GetClient<TClient>() where TClient : IBaseClient;
}
