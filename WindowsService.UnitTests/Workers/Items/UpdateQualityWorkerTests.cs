using AwesomeAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Moq;
using WindowsService.ApiClient;
using WindowsService.Settings;
using WindowsService.Workers.Items;

namespace WindowsService.UnitTests.Workers.Items;

public class UpdateQualityWorkerTests
{
    private static readonly TimeSpan ShortInterval = TimeSpan.FromMilliseconds(10);
    private static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(5);

    [Fact]
    public async Task ExecuteAsync_OnEachTick_CallsUpdateQualityEndpoint()
    {
        // Arrange
        var callCount = 0;
        var adapter = new Mock<IRequestAdapter>();
        adapter.SetupProperty(a => a.BaseUrl, "https://localhost:7146");
        adapter.Setup(a => a.SendNoContentAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .Callback(() => callCount++)
            .Returns(Task.CompletedTask);

        var client = new GildedRoseApiClient(adapter.Object);
        var options = Options.Create(new JobOptions { Interval = ShortInterval });
        var worker = new UpdateQualityWorker(client, options, NullLogger<UpdateQualityWorker>.Instance);

        // Act
        await worker.StartAsync(CancellationToken.None);

        var deadline = DateTime.UtcNow + WaitTimeout;
        while (callCount < 2 && DateTime.UtcNow < deadline)
        {
            await Task.Delay(10);
        }

        await worker.StopAsync(CancellationToken.None);

        // Assert
        callCount.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCallFails_ContinuesOnNextTick()
    {
        // Arrange
        var callCount = 0;
        var adapter = new Mock<IRequestAdapter>();
        adapter.Setup(a => a.SendNoContentAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                callCount++;
                return callCount == 1
                    ? Task.FromException(new HttpRequestException("API unavailable"))
                    : Task.CompletedTask;
            });

        var client = new GildedRoseApiClient(adapter.Object);
        var options = Options.Create(new JobOptions { Interval = ShortInterval });
        var worker = new UpdateQualityWorker(client, options, NullLogger<UpdateQualityWorker>.Instance);

        // Act
        await worker.StartAsync(CancellationToken.None);

        var deadline = DateTime.UtcNow + WaitTimeout;
        while (callCount < 2 && DateTime.UtcNow < deadline)
        {
            await Task.Delay(10);
        }

        await worker.StopAsync(CancellationToken.None);

        // Assert
        callCount.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task ExecuteAsync_WhenStopped_ExitsWithoutThrowing()
    {
        // Arrange
        var adapter = new Mock<IRequestAdapter>();
        adapter.Setup(a => a.SendNoContentAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var client = new GildedRoseApiClient(adapter.Object);
        var options = Options.Create(new JobOptions { Interval = TimeSpan.FromMinutes(1) });
        var worker = new UpdateQualityWorker(client, options, NullLogger<UpdateQualityWorker>.Instance);

        // Act
        await worker.StartAsync(CancellationToken.None);
        var stop = worker.StopAsync(CancellationToken.None);

        // Assert
        await FluentActions.Awaiting(() => stop).Should().NotThrowAsync();
    }
}