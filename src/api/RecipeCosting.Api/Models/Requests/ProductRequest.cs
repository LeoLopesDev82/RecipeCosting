using System.ComponentModel.DataAnnotations;

namespace RecipeCosting.Api.Models.Requests;

/// <summary>
/// A product and its recipe, as the client sends them. No cost and no price:
/// both are worked out here.
/// </summary>
public class ProductRequest : IValidatableObject
{
    /// <summary>
    /// The version the caller read. A write built on an older one is refused, so that
    /// two people editing the same record do not silently overwrite each other.
    /// </summary>
    public int Version { get; set; }

    /// <summary>Name shown to the baker, such as "Brigadeiro".</summary>
    [Required(ErrorMessage = "The name is required.")]
    [MaxLength(120, ErrorMessage = "The name cannot exceed 120 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Minutes of work one of these takes.</summary>
    [Range(0, 10_000, ErrorMessage = "The preparation time cannot be negative.")]
    public decimal PrepMinutes { get; set; }

    /// <summary>Markup of its own. Leave it out to follow the baker's default.</summary>
    [Range(0, 1000, ErrorMessage = "The markup must be between 0 and 1000.")]
    public decimal? Markup { get; set; }

    /// <summary>The ingredients that go into one of these.</summary>
    [MinLength(1, ErrorMessage = "A product needs at least one ingredient.")]
    public List<ProductLineRequest> Lines { get; set; } = [];

    /// <summary>
    /// The same ingredient twice would be two lines of one thing, which hides
    /// how much of it the recipe really uses.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var repeated = Lines
            .GroupBy(line => line.IngredientId)
            .Any(group => group.Count() > 1);

        if (repeated)
            yield return new ValidationResult(
                "The same ingredient cannot appear twice in one recipe.",
                [nameof(Lines)]);
    }
}
