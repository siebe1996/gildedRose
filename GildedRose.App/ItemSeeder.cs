using GildedRose.App.ApiClient;
using Microsoft.Extensions.Logging;

namespace GildedRose.App;

public sealed class ItemSeeder(GildedRoseApiClient apiClient, ILogger<ItemSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await apiClient.Api.Items.Add.PostAsync(new() { Items = SampleItems.Create() }, cancellationToken: cancellationToken);
        logger.LogInformation("Items sent to the API.");
    }
}
