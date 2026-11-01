using System.ComponentModel.DataAnnotations;
using Api.Items.Requests;
using AwesomeAssertions;

namespace Api.UnitTests.Items.Requests;

public class AddItemsRequestTests
{
    [Fact]
    public void Validate_WithValidItems_HasNoErrors()
    {
        // Arrange
        var request = new AddItemsRequest
        {
            Items =
            [
                new AddItemsRequest.AddItemsRequestItem
                {
                    Name = "Ring of Cleansening Code",
                    SellIn = 10,
                    Quality = 20
                }
            ]
        };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(request, new ValidationContext(request), results, validateAllProperties: true);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithNullItems_HasErrorForItems()
    {
        // Arrange
        var request = new AddItemsRequest { Items = null };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(request, new ValidationContext(request), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(AddItemsRequest.Items));
    }

    [Fact]
    public void Validate_WithEmptyItems_HasErrorForItems()
    {
        // Arrange
        var request = new AddItemsRequest { Items = [] };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(request, new ValidationContext(request), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(AddItemsRequest.Items));
    }

    [Fact]
    public void Validate_ItemWithNullName_HasErrorForName()
    {
        // Arrange
        var item = new AddItemsRequest.AddItemsRequestItem
        {
            Name = null,
            SellIn = 10,
            Quality = 20
        };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(item, new ValidationContext(item), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(AddItemsRequest.AddItemsRequestItem.Name));
    }

    [Fact]
    public void Validate_ItemWithWhitespaceName_HasErrorForName()
    {
        // Arrange
        var item = new AddItemsRequest.AddItemsRequestItem
        {
            Name = "   ",
            SellIn = 10,
            Quality = 20
        };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(item, new ValidationContext(item), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(AddItemsRequest.AddItemsRequestItem.Name));
    }

    [Fact]
    public void Validate_ItemWithNullSellIn_HasErrorForSellIn()
    {
        // Arrange
        var item = new AddItemsRequest.AddItemsRequestItem
        {
            Name = "Good Wine",
            SellIn = null,
            Quality = 0
        };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(item, new ValidationContext(item), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(AddItemsRequest.AddItemsRequestItem.SellIn));
    }

    [Fact]
    public void Validate_ItemWithNullQuality_HasErrorForQuality()
    {
        // Arrange
        var item = new AddItemsRequest.AddItemsRequestItem
        {
            Name = "Good Wine",
            SellIn = 2,
            Quality = null
        };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(item, new ValidationContext(item), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(AddItemsRequest.AddItemsRequestItem.Quality));
    }

    [Fact]
    public void Validate_ItemWithNegativeQuality_HasErrorForQuality()
    {
        // Arrange
        var item = new AddItemsRequest.AddItemsRequestItem
        {
            Name = "Good Wine",
            SellIn = 2,
            Quality = -1
        };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(item, new ValidationContext(item), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(AddItemsRequest.AddItemsRequestItem.Quality));
    }

    [Fact]
    public void Validate_ItemWithZeroQuality_HasNoErrors()
    {
        // Arrange
        var item = new AddItemsRequest.AddItemsRequestItem
        {
            Name = "Good Wine",
            SellIn = 2,
            Quality = 0
        };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(item, new ValidationContext(item), results, validateAllProperties: true);

        // Assert
        results.Should().BeEmpty();
    }
}