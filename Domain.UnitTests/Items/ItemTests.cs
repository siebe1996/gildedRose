using AwesomeAssertions;
using Domain.Items;

namespace Domain.UnitTests.Items;

public class ItemTests
{
    [Fact]
    public void UpdateQuality_NormalItemBeforeSellBy_DecreasesQualityAndSellIn()
    {
        // Arrange
        var item = new Item("Ring of Cleansening Code", 10, 20);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(19);
        item.SellIn.Should().Be(9);
    }

    [Fact]
    public void UpdateQuality_NormalItemAfterSellBy_DecreasesQualityTwice()
    {
        // Arrange
        var item = new Item("Ring of Cleansening Code", 0, 6);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(4);
        item.SellIn.Should().Be(-1);
    }

    [Fact]
    public void UpdateQuality_NormalItemWithZeroQuality_DoesNotGoNegative()
    {
        // Arrange
        var item = new Item("Ring of Cleansening Code", 5, 0);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(0);
    }

    [Fact]
    public void UpdateQuality_GoodWine_IncreasesQuality()
    {
        // Arrange
        var item = new Item("Good Wine", 2, 0);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(1);
        item.SellIn.Should().Be(1);
    }

    [Fact]
    public void UpdateQuality_BackstageMoreThan10Days_IncreasesByOne()
    {
        // Arrange
        var item = new Item("Backstage passes for Re:factor", 15, 20);

        // Act
        item.UpdateQuality();
        
        // Assert
        item.Quality.Should().Be(21);
    }

    [Fact]
    public void UpdateQuality_Backstage10DaysOrLess_IncreasesByTwo()
    {
        // Arrange
        var item = new Item("Backstage passes for Re:factor", 10, 20);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(22);
    }

    [Fact]
    public void UpdateQuality_Backstage5DaysOrLess_IncreasesByThree()
    {
        // Arrange
        var item = new Item("Backstage passes for HAXX", 5, 20);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(23);
    }

    [Fact]
    public void UpdateQuality_BackstageAfterConcert_DropsToZero()
    {
        // Arrange
        var item = new Item("Backstage passes for HAXX", 0, 20);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(0);
        item.SellIn.Should().Be(-1);
    }

    [Fact]
    public void UpdateQuality_Sulfuras_NeverChanges()
    {
        // Arrange
        var item = new Item("B-DAWG Keychain", 0, 80);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(80);
        item.SellIn.Should().Be(0);
    }

    [Fact]
    public void UpdateQuality_Quality_NeverExceeds50ForGoodWine()
    {
        // Arrange
        var item = new Item("Good Wine", 5, 50);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(50);
    }

    [Fact]
    public void UpdateQuality_Conjured_DecreasesByTwo()
    {
        // Arrange
        var item = new Item("Conjured", 5, 10);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(8);
        item.SellIn.Should().Be(4);
    }

    [Fact]
    public void UpdateQuality_FooWithZeroQualityAndSellInZero_KeepsQualityAtZeroAndDecreasesSellIn()
    {
        // Arrange
        var item = new Item("foo", 0, 0);

        // Act
        item.UpdateQuality();

        // Assert
        item.Quality.Should().Be(0);
        item.SellIn.Should().Be(-1);
    }
}