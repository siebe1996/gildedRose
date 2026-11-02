using GildedRose.App.ApiClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace GildedRose.App.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGildedRoseApi(this IServiceCollection services, string baseUrl)
    {
        services.AddSingleton<IRequestAdapter>(_ => new HttpClientRequestAdapter(new AnonymousAuthenticationProvider())
        {
            BaseUrl = baseUrl
        });
        services.AddSingleton<GildedRoseApiClient>();

        return services;
    }
}
