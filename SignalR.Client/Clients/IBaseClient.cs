namespace SignalR.Client.Clients;

public interface IBaseClient : IAsyncDisposable
{
    bool IsDisposed { get; }

    Task ConnectWithRetryAsync(TimeSpan retryInterval, TimeSpan maxRetryInterval, CancellationToken cancellationToken);
}
