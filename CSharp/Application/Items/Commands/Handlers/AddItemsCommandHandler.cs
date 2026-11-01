using Domain.Items;

namespace Application.Items.Commands.Handlers;

internal sealed class AddItemsCommandHandler(IItemRepository itemRepository) : ICommandHandler<AddItemsCommand>
{
    public Task Handle(AddItemsCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items
            .Select(i => new Item(i.Name, i.SellIn, i.Quality))
            .ToList();

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (items.Count == 0)
        {
            return Task.CompletedTask;
        }

        return itemRepository.AddRangeAsync(items, cancellationToken);
    }
}