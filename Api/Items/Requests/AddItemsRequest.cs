using System.ComponentModel.DataAnnotations;

namespace Api.Items.Requests;

public class AddItemsRequest
{
    [Required] 
    [MinLength(1)]
    public List<AddItemsRequestItem>? Items { get; init; }
    
    public class AddItemsRequestItem : IValidatableObject
    {
        [Required]
        public string? Name { get; init; }

        [Required]
        public int? SellIn { get; init; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Quality { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name is not null && string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be only whitespace.", [nameof(Name)]);
            }
        }
    }
}