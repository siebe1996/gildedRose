namespace Api.Items.Hubs;

public interface IItemsNotifier
{
    Task NotifyItemsUpdatedAsync(CancellationToken cancellationToken = default);
}