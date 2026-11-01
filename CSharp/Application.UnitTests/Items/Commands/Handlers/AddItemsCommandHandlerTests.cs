using Application.Items.Commands;
using Application.Items.Commands.Handlers;
using Domain.Items;
using Models.Items;
using Moq;

namespace Application.UnitTests.Items.Commands.Handlers;

public class AddItemsCommandHandlerTests
{
    private readonly Mock<IItemRepository> _itemRepositoryMock = new();
    private readonly AddItemsCommandHandler _handler;

    public AddItemsCommandHandlerTests()
    {
        _handler = new AddItemsCommandHandler(_itemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithItems_CallsAddRangeAsyncOnce()
    {
        // Arrange
        var command = new AddItemsCommand(
        [
            new ItemRequestModel("Ring of Cleansening Code", 10, 20),
            new ItemRequestModel("Good Wine", 2, 0)
        ]);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _itemRepositoryMock.Verify(
            r => r.AddRangeAsync(It.Is<IEnumerable<Item>>(items => items.Count() == 2), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}