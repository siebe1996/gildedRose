using System.ComponentModel.DataAnnotations;
using AwesomeAssertions;
using WindowsService.Settings;

namespace WindowsService.UnitTests.Settings;

public class JobOptionsTests
{
    [Fact]
    public void Validate_WithPositiveInterval_HasNoErrors()
    {
        // Arrange
        var options = new JobOptions { Interval = TimeSpan.FromSeconds(30) };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(options, new ValidationContext(options), results, validateAllProperties: true);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithIntervalOfThreeDays_HasNoErrors()
    {
        // Arrange
        var options = new JobOptions { Interval = TimeSpan.FromDays(3) };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(options, new ValidationContext(options), results, validateAllProperties: true);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithNullInterval_HasErrorForInterval()
    {
        // Arrange
        var options = new JobOptions { Interval = null };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(options, new ValidationContext(options), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(JobOptions.Interval));
    }

    [Fact]
    public void Validate_WithZeroInterval_HasErrorForInterval()
    {
        // Arrange
        var options = new JobOptions { Interval = TimeSpan.Zero };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(options, new ValidationContext(options), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(JobOptions.Interval));
    }

    [Fact]
    public void Validate_WithNegativeInterval_HasErrorForInterval()
    {
        // Arrange
        var options = new JobOptions { Interval = TimeSpan.FromSeconds(-5) };
        var results = new List<ValidationResult>();

        // Act
        Validator.TryValidateObject(options, new ValidationContext(options), results, validateAllProperties: true);

        // Assert
        results.Should().ContainSingle()
            .Which.MemberNames.Should().Contain(nameof(JobOptions.Interval));
    }
}