using Application.Items.Commands;
using Application.Items.Validators;
using AwesomeAssertions;
using FluentValidation.TestHelper;
using Models.Items;

namespace Application.UnitTests.Items.Validators;

public class AddItemsCommandValidatorTests
{
    private readonly AddItemsCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidItems_HasNoErrors()
    {
        // Arrange
        var command = new AddItemsCommand(
        [
            new ItemRequestModel("Ring of Cleansening Code", 10, 20),
            new ItemRequestModel("Good Wine", 2, 0)
        ]);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithNullItems_HasValidationError()
    {
        // Arrange
        var command = new AddItemsCommand(null!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Items);
    }

    [Fact]
    public void Validate_WithEmptyItems_HasValidationError()
    {
        // Arrange
        var command = new AddItemsCommand([]);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Items);
    }

    [Fact]
    public void Validate_WithWhitespaceName_HasValidationError()
    {
        // Arrange
        var command = new AddItemsCommand(
        [
            new ItemRequestModel("   ", 10, 20)
        ]);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Name");
    }

    [Fact]
    public void Validate_WithNegativeQuality_HasValidationError()
    {
        // Arrange
        var command = new AddItemsCommand(
        [
            new ItemRequestModel("Good Wine", 2, -1)
        ]);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quality");
    }

    [Fact]
    public void Validate_WithZeroQuality_HasNoErrors()
    {
        // Arrange
        var command = new AddItemsCommand(
        [
            new ItemRequestModel("Good Wine", 2, 0)
        ]);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}