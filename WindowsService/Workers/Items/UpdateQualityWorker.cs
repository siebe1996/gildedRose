using Microsoft.Extensions.Options;
using WindowsService.ApiClient;
using WindowsService.Settings;

namespace WindowsService.Workers.Items;

public sealed class UpdateQualityWorker(GildedRoseApiClient client, IOptions<JobOptions> options, ILogger<UpdateQualityWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(options.Value.Interval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await client.Api.Items.UpdateQuality.PostAsync(cancellationToken: stoppingToken);
                logger.LogInformation("Quality updated.");
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // Log and continue so one failed tick doesn't stop the service.
                logger.LogError(ex, "Quality update failed. Retrying on the next tick.");
            }
        }
    }
}