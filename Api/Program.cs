using Api.Items.Hubs;
using Api.Items.Requests;
using Application;
using Application.DependencyInjection;
using Application.Items.Commands;
using Application.Items.Queries;
using Dal.DependencyInjection;
using Microsoft.AspNetCore.Http.HttpResults;
using Models.Items;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationHandlers();
builder.Services.AddDal();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IItemsNotifier, ItemsNotifier>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/items/add", async Task<Created> (AddItemsRequest request, ICommandHandler<AddItemsCommand> handler, IItemsNotifier notifier, CancellationToken cancellationToken) =>
    {
        var itemsToBeCreated = request.Items!.Select(i => new ItemRequestModel(i.Name!, i.SellIn!.Value, i.Quality!.Value)).ToList();
        await handler.Handle(new AddItemsCommand(itemsToBeCreated), cancellationToken);
        await notifier.NotifyItemsUpdatedAsync(cancellationToken);

        return TypedResults.Created();

    }).WithName("AddItems");

app.MapPost("/api/items/update-quality", async (ICommandHandler<UpdateQualityCommand> handler, IItemsNotifier notifier, CancellationToken cancellationToken) =>
{
    await handler.Handle(new UpdateQualityCommand(), cancellationToken);
    await notifier.NotifyItemsUpdatedAsync(cancellationToken);

    return TypedResults.NoContent();
}).WithName("UpdateQuality");

app.MapGet("/api/items", async (IQueryHandler<GetItemsQuery, List<ItemModel>> handler, CancellationToken cancellationToken) =>
{
    var items = await handler.Handle(new GetItemsQuery(), cancellationToken);

    return TypedResults.Ok(items);
}).WithName("GetAllItems");

app.MapHub<ItemsSignalRHub>("/hubs/items");

app.Run();
