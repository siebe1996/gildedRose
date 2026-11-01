using Models.Items;

namespace Application.Items.Queries.Handlers;

internal class GetItemsQueryHandler(IItemsModelRepository itemsModelRepository)
    : IQueryHandler<GetItemsQuery, List<ItemModel>>
{
    public Task<List<ItemModel>> Handle(GetItemsQuery query, CancellationToken cancellationToken)
    {
        return itemsModelRepository.GetAllItemsAsync(cancellationToken);
    }
}