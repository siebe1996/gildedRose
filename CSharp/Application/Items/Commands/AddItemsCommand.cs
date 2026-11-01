using Models.Items;

namespace Application.Items.Commands;

public record AddItemsCommand(List<ItemRequestModel> Items): ICommand;