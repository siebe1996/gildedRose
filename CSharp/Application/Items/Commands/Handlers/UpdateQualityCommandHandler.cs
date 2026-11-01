using Domain.Items;

namespace Application.Items.Commands.Handlers;

internal sealed class UpdateQualityCommandHandler(IItemRepository repository) : ICommandHandler<UpdateQualityCommand>
{
    public async Task Handle(UpdateQualityCommand command, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllAsync(cancellationToken);

        foreach (var item in items)
        {
            item.UpdateQuality();
        }
    }
}