using GildedRose.App;
using GildedRose.App.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.Client.DependencyInjection;

Console.WriteLine("OMGHAI!");

// Add a host for DI
var builder = Host.CreateApplicationBuilder(args);

var baseUrl = builder.Configuration["services:api:https:0"]
              ?? throw new InvalidOperationException("API address was not injected by the AppHost.");

builder.Services.AddGildedRoseApi(baseUrl);
builder.Services.AddSignalRClients(baseUrl);
builder.Services.AddSingleton(Console.Out);
builder.Services.AddSingleton<IItemsService, ItemsService>();
builder.Services.AddSingleton<ItemSeeder>();
builder.Services.AddSingleton<ItemsPrinter>();

using var host = builder.Build();

var seeder = host.Services.GetRequiredService<ItemSeeder>();
var itemsService = host.Services.GetRequiredService<IItemsService>();
var printer = host.Services.GetRequiredService<ItemsPrinter>();

// Seed the items to the API.
await seeder.SeedAsync();

var shutdown = new TaskCompletionSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    shutdown.TrySetResult();
};

// Subscribe to the changed event of items service and print the items when they change. This will also print the items once at startup
printer.Subscribe();
// Connect to the SignalR hub to receive updates when items change
await itemsService.ConnectAsync();
Console.WriteLine("Connected to the hub. Waiting for updates. Press Ctrl+C to stop.");

await printer.PrintAsync();

await shutdown.Task;
