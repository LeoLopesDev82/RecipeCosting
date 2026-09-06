using RecipeCosting.Api.Models.Enums;

namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// One ingredient inside a recipe, with what that quantity costs today.
/// </summary>
public class ProductLineResponse
{
    /// <summary>Identifier of the ingredient used.</summary>
    public int IngredientId { get; set; }

    /// <summary>Name of the ingredient, so a list needs no second call.</summary>
    public string IngredientName { get; set; } = string.Empty;

    /// <summary>How much is used.</summary>
    public decimal Quantity { get; set; }

    /// <summary>The unit that quantity is measured in.</summary>
    public PackageUnit Unit { get; set; }

    /// <summary>What that quantity costs at the ingredient's current price.</summary>
    public decimal Cost { get; set; }
}
