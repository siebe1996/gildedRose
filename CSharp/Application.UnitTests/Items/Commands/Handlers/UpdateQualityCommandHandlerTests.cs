using Application.Items.Commands;
using Application.Items.Commands.Handlers;
using AwesomeAssertions;
using Domain.Items;
using Moq;

namespace Application.UnitTests.Items.Commands.Handlers;

public class UpdateQualityCommandHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock = new();
    private readonly UpdateQualityCommandHandler _handler;

    public UpdateQualityCommandHandlerTests()
    {
        _handler = new UpdateQualityCommandHandler(_itemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithItems_CallsGetAllAsyncOnce()
    {
        // Arrange
        _itemRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _handler.Handle(new UpdateQualityCommand(), CancellationToken.None);

        // Assert
        _itemRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}