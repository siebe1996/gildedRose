namespace Models.Items;

public interface IItemsModelRepository
{
    Task<List<ItemModel>> GetAllItemsAsync(CancellationToken cancellationToken = default);
}