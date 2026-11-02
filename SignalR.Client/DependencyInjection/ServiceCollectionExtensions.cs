using Microsoft.Extensions.DependencyInjection;
using SignalR.Client.Clients.Items;

namespace SignalR.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSignalRClients(this IServiceCollection services, string baseUrl)
    {
        services.AddSingleton(new ApiOptions(baseUrl));
        services.AddSingleton<ISignalRClientFactory, SignalRClientFactory>();
        services.AddTransient<ItemsHubClient>();
        services.AddTransient<IItemsNotificationClient, ItemsHubClient>();

        return services;
    }
}
