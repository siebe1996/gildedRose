using Models.Items;

namespace Application.Items.Queries;

public record GetItemsQuery() : IQuery<List<ItemModel>>;