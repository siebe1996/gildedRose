using AwesomeAssertions;
using Dal.Items.Repositories;
using Domain.Items;

namespace Dal.UnitTests.Items.Repositories;

public class ItemRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_WhenEmpty_ReturnsEmptyList()
    {
        // Arrange
        var repository = new ItemRepository();

        // Act
        var result = await repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllAddedItems()
    {
        // Arrange
        var repository = new ItemRepository();
        var item1 = new Item("Apple", 10, 20);
        var item2 = new Item("Wine", 5, 3);
        await repository.AddAsync(item1, TestContext.Current.CancellationToken);
        await repository.AddAsync(item2, TestContext.Current.CancellationToken);

        // Act
        var result = await repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(item1);
        result.Should().Contain(item2);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsSnapshot_NotAffectedByLaterAdds()
    {
        // Arrange
        var repository = new ItemRepository();
        await repository.AddAsync(new Item("Apple", 10, 20), TestContext.Current.CancellationToken);
        var snapshot = await repository.GetAllAsync(TestContext.Current.CancellationToken);

        // Act
        await repository.AddAsync(new Item("Wine", 5, 3), TestContext.Current.CancellationToken);

        // Assert
        snapshot.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAsync_WhenItemExists_ReturnsItem()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item("Apple", 10, 20);
        await repository.AddAsync(item, TestContext.Current.CancellationToken);

        // Act
        var result = await repository.GetAsync(item.Id, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeSameAs(item);
    }

    [Fact]
    public async Task GetAsync_WhenItemDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repository = new ItemRepository();
        await repository.AddAsync(new Item("Apple", 10, 20), TestContext.Current.CancellationToken);

        // Act
        var result = await repository.GetAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_AddsItemToRepository()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item("Apple", 10, 20);

        // Act
        await repository.AddAsync(item, TestContext.Current.CancellationToken);

        // Assert
        var all = await repository.GetAllAsync(TestContext.Current.CancellationToken);
        all.Should().ContainSingle().Which.Should().BeSameAs(item);
    }

    [Fact]
    public async Task AddRangeAsync_AddsAllItemsToRepository()
    {
        // Arrange
        var repository = new ItemRepository();
        var items = new List<Item>
        {
            new("Apple", 10, 20),
            new("Wine", 5, 3),
            new("Sulfuras", 0, 80)
        };

        // Act
        await repository.AddRangeAsync(items, TestContext.Current.CancellationToken);

        // Assert
        var all = await repository.GetAllAsync(TestContext.Current.CancellationToken);
        all.Should().HaveCount(items.Count);
        all.Should().Contain(items);
    }

    [Fact]
    public async Task AddRangeAsync_WithEmptyCollection_LeavesRepositoryEmpty()
    {
        // Arrange
        var repository = new ItemRepository();

        // Act
        await repository.AddRangeAsync([], TestContext.Current.CancellationToken);

        // Assert
        var all = await repository.GetAllAsync(TestContext.Current.CancellationToken);
        all.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllItemsAsync_WhenEmpty_ReturnsEmptyList()
    {
        // Arrange
        var repository = new ItemRepository();

        // Act
        var result = await repository.GetAllItemsAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllItemsAsync_MapsItemsToItemModels()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item("Apple", 10, 20);
        await repository.AddAsync(item, TestContext.Current.CancellationToken);
    
        // Act
        var result = await repository.GetAllItemsAsync(TestContext.Current.CancellationToken);
    
        // Assert
        var model = result.Should().ContainSingle().Subject;
        model.Id.Should().Be(item.Id);
        model.Name.Should().Be(item.Name);
        model.SellIn.Should().Be(item.SellIn);
        model.Quality.Should().Be(item.Quality);
    }

    [Fact]
    public async Task GetAsync_WithCancelledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        var repository = new ItemRepository();
        var cancelledToken = new CancellationToken(canceled: true);

        // Act
        var act = () => repository.GetAsync(Guid.NewGuid(), cancelledToken);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}