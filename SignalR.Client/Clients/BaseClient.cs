using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Client.Clients;

public abstract class BaseClient(string hubUrl) : IBaseClient
{
    protected HubConnection Connection { get; } = new HubConnectionBuilder()
        .WithUrl(hubUrl)
        .WithAutomaticReconnect()
        .Build();

    public bool IsDisposed { get; private set; }

    public async Task ConnectWithRetryAsync(TimeSpan retryInterval, TimeSpan maxRetryInterval, CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                await Connection.StartAsync(cancellationToken);
                return;
            }
            catch (Exception) when (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(retryInterval, cancellationToken);

                retryInterval = retryInterval >= maxRetryInterval
                    ? maxRetryInterval
                    : retryInterval + retryInterval;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (IsDisposed)
        {
            return;
        }
        await Connection.DisposeAsync();
        GC.SuppressFinalize(this);
        IsDisposed = true;
    }
}
