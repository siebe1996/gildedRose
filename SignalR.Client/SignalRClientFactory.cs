using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Client.Clients;

namespace SignalR.Client;

public sealed class SignalRClientFactory(IServiceProvider services) : ISignalRClientFactory
{
    private readonly ConcurrentDictionary<Type, IBaseClient> _clients = new();

    public TClient GetClient<TClient>() where TClient : IBaseClient
    {
        if (_clients.TryGetValue(typeof(TClient), out var existing))
        {
            if (!existing.IsDisposed)
            {
                return (TClient) existing;
            }

            _clients.TryRemove(typeof(TClient), out _);
        }

        return (TClient) _clients.GetOrAdd(typeof(TClient), _ => services.GetRequiredService<TClient>());
    }
}
