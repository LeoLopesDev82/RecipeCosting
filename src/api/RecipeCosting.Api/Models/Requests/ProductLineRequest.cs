using System.ComponentModel.DataAnnotations;

namespace RecipeCosting.Api.Models.Requests;

/// <summary>
/// One ingredient of a recipe, as the client sends it. The cost is never sent:
/// the API reads the ingredient's current price.
/// </summary>
public class ProductLineRequest
{
    /// <summary>Identifier of the ingredient used.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "Every line has to name an ingredient.")]
    public int IngredientId { get; set; }

    /// <summary>How much is used, in the ingredient's package unit.</summary>
    [Range(0.001, 1_000_000, ErrorMessage = "Every quantity must be greater than zero.")]
    public decimal Quantity { get; set; }
}
