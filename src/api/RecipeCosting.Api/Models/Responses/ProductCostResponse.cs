namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// What a product costs to make and what it should sell for.
/// </summary>
public class ProductCostResponse
{
    /// <summary>What the recipe's ingredients cost together.</summary>
    public decimal Ingredients { get; set; }

    /// <summary>What the preparation time costs at the baker's hourly rate.</summary>
    public decimal Labour { get; set; }

    /// <summary>Ingredients and labour together.</summary>
    public decimal Total { get; set; }

    /// <summary>The markup actually applied.</summary>
    public decimal Markup { get; set; }

    /// <summary>True when the markup came from the baker's default.</summary>
    public bool Inherited { get; set; }

    /// <summary>
    /// What to charge. The markup goes on top of the cost, and the card fee and
    /// the tax are added back on, because they come off the selling price.
    /// </summary>
    public decimal Price { get; set; }
}
