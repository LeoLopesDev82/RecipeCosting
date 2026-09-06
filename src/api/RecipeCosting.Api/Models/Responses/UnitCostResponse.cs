namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// The price of an ingredient normalised to a single measure, so that a recipe
/// can multiply it by the quantity it uses.
/// </summary>
public class UnitCostResponse
{
    /// <summary>Cost of one whole measure, such as one kilogram.</summary>
    public decimal Amount { get; set; }

    /// <summary>The measure the amount refers to: kg, L or unit.</summary>
    public string Label { get; set; } = string.Empty;
}
