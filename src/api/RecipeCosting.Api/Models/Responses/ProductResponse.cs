namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// A product with its recipe and everything the pricing screen shows.
/// </summary>
public class ProductResponse
{
    /// <summary>The version of the record, to be sent back when replacing it.</summary>
    public int Version { get; set; }

    /// <summary>Identifier of the product.</summary>
    public int Id { get; set; }

    /// <summary>Name shown to the baker.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Minutes of work the whole recipe takes.</summary>
    public decimal PrepMinutes { get; set; }

    /// <summary>How many units the recipe makes.</summary>
    public int Yield { get; set; }

    /// <summary>Markup of its own, or null when it follows the baker's default.</summary>
    public decimal? Markup { get; set; }

    /// <summary>The recipe, with the cost of each line.</summary>
    public List<ProductLineResponse> Lines { get; set; } = [];

    /// <summary>What it costs and what it should sell for.</summary>
    public ProductCostResponse Cost { get; set; } = new();
}
