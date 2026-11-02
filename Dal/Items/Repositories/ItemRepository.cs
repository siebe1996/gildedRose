using Domain.Items;
using Domain.SeedWork;
using Models.Items;

namespace Dal.Items.Repositories;

public class ItemRepository : IItemRepository, IItemsModelRepository
{
    private readonly Lock _lock = new();
    private readonly List<Item> _items = [];

    public async Task<IReadOnlyList<Item>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(5, cancellationToken);

        lock (_lock)
        {
            return [.. _items];
        }
    }

    public async Task<Item?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.Delay(5, cancellationToken);

        lock (_lock)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }
    }

    public async Task AddAsync(Item item, CancellationToken cancellationToken = default)
    {
        await Task.Delay(5, cancellationToken);

        lock (_lock)
        {
            _items.Add(item);
        }
    }

    public async Task AddRangeAsync(IEnumerable<Item> items, CancellationToken cancellationToken = default)
    {
        await Task.Delay(5, cancellationToken);

        lock (_lock)
        {
            _items.AddRange(items);
        }
    }

    public async Task<List<ItemModel>> GetAllItemsAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(5, cancellationToken);
        lock (_lock)
        {
            return
            [
                .. _items.Select(i => new ItemModel
                (
                    i.Id,
                    i.Name,
                    i.SellIn,
                    i.Quality
                ))
            ];
        }
    }
}