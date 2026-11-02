using Application.Items.Queries;
using Application.Items.Queries.Handlers;
using AwesomeAssertions;
using Models.Items;
using Moq;

namespace Application.UnitTests.Items.Queries.Handlers;

public class GetItemsQueryHandlerTests
{
    private readonly Mock<IItemsModelRepository> _itemsModelRepositoryMock = new();
    private readonly GetItemsQueryHandler _handler;

    public GetItemsQueryHandlerTests()
    {
        _handler = new GetItemsQueryHandler(_itemsModelRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithItems_ReturnsItemsFromRepository()
    {
        // Arrange
        var items = new List<ItemModel>
        {
            new(Guid.NewGuid(), "Ring of Cleansening Code", 10, 20),
            new(Guid.NewGuid(), "Good Wine", 2, 0)
        };

        _itemsModelRepositoryMock
            .Setup(r => r.GetAllItemsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        // Act
        var result = await _handler.Handle(new GetItemsQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(items);
    }
}