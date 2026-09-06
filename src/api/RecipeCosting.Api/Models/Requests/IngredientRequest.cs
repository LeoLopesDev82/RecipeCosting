using System.ComponentModel.DataAnnotations;
using RecipeCosting.Api.Models.Enums;

namespace RecipeCosting.Api.Models.Requests;

/// <summary>
/// What the client sends to create or change an ingredient. The unit cost is
/// never sent: the API works it out from the package.
/// </summary>
public class IngredientRequest
{
    /// <summary>Name shown to the baker, such as "Condensed milk".</summary>
    [Required(ErrorMessage = "The name is required.")]
    [MaxLength(120, ErrorMessage = "The name cannot exceed 120 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>How much the package holds, in the unit below.</summary>
    [Range(0.01, 1_000_000, ErrorMessage = "The package size must be greater than zero.")]
    public decimal PackageSize { get; set; }

    /// <summary>The unit the package is measured in: g, ml or un.</summary>
    [Required(ErrorMessage = "The package unit is required.")]
    [EnumDataType(typeof(PackageUnit), ErrorMessage = "The package unit must be g, ml or un.")]
    public PackageUnit PackageUnit { get; set; }

    /// <summary>What the whole package costs.</summary>
    [Range(0.01, 1_000_000, ErrorMessage = "The package price must be greater than zero.")]
    public decimal PackagePrice { get; set; }
}
