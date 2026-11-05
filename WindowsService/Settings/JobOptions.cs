using System.ComponentModel.DataAnnotations;

namespace WindowsService.Settings;

public sealed class JobOptions : IValidatableObject
{
    public const string SectionName = "Job";

    [Required]
    public TimeSpan? Interval { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Interval is null)
        {
            yield break; // Already reported by [Required].
        }

        var interval = Interval.Value;

        if (interval <= TimeSpan.Zero)
        {
            yield return new ValidationResult(
                "Job:Interval must be greater than zero.",
                [nameof(Interval)]);
        }
    }
}