using GildedRose.App.ApiClient;
using GildedRose.App.ApiClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

Console.WriteLine("OMGHAI!");

var baseUrl = Environment.GetEnvironmentVariable("services__api__https__0")
              ?? throw new InvalidOperationException("API address was not injected by the AppHost.");

var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider())
{
    BaseUrl = baseUrl
};
var client = new GildedRoseApiClient(adapter);

var day = 0;

await client.Api.Items.Add.PostAsync(CreateRequest());
Console.WriteLine("Items sent to the API.");

await using var hub = new HubConnectionBuilder()
    .WithUrl(baseUrl + "/hubs/items")
    .WithAutomaticReconnect()
    .Build();

hub.On("ItemsUpdated", async () =>
{
    await PrintItemsAsync();
});

await hub.StartAsync();
Console.WriteLine("Connected to the hub. Waiting for updates. Press Ctrl+C to stop.");

await PrintItemsAsync();

await Task.Delay(Timeout.Infinite);
return;

static AddItemsRequest CreateRequest()
{
    var items = new List<AddItemsRequestItem>
    {
        new() { Name = "Ring of Cleansening Code", SellIn = 10, Quality = 20 },
        new() { Name = "Good Wine", SellIn = 2, Quality = 0 },
        new() { Name = "Elixir of the SOLID", SellIn = 5, Quality = 7 },
        new() { Name = "B-DAWG Keychain", SellIn = 0, Quality = 80 },
        new() { Name = "B-DAWG Keychain", SellIn = -1, Quality = 80 },
        new() { Name = "Backstage passes for Re:factor", SellIn = 15, Quality = 20 },
        new() { Name = "Backstage passes for Re:factor", SellIn = 10, Quality = 49 },
        new() { Name = "Backstage passes for HAXX", SellIn = 5, Quality = 49 },
        // these smelly items do not work properly yet
        new() { Name = "Duplicate Code", SellIn = 3, Quality = 6 },
        new() { Name = "Long Methods", SellIn = 3, Quality = 6 },
        new() { Name = "Ugly Variable Names", SellIn = 3, Quality = 6 }
    };

    return new AddItemsRequest { Items = items };
}

async Task PrintItemsAsync()
{
    try
    {
        var items = await client.Api.Items.GetAsync() ?? [];

        Console.WriteLine("-------- day " + day + " --------");
        Console.WriteLine("name, sellIn, quality");

        foreach (var item in items)
        {
            Console.WriteLine(item.Name + ", " + item.SellIn + ", " + item.Quality);
        }

        Console.WriteLine();
        day++;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Could not load items: " + ex.Message);
    }
}