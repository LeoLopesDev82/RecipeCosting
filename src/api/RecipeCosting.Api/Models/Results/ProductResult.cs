using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Models.Results;

/// <summary>
/// Why a write against a product ended the way it did.
/// </summary>
public enum ProductOutcome
{
    /// <summary>The product was stored, or priced, and is being returned.</summary>
    Priced,

    /// <summary>No product carries the identifier that was asked for.</summary>
    ProductNotFound,

    /// <summary>A recipe line names an ingredient that is not in the pantry.</summary>
    IngredientNotFound,

    /// <summary>Someone else replaced the product after this caller read it.</summary>
    Stale,
}

/// <summary>
/// The outcome of a write, and the product when there is one to return.
/// </summary>
public class ProductResult
{
    private ProductResult(ProductOutcome outcome, ProductResponse? product, int missingIngredientId)
    {
        Outcome = outcome;
        Product = product;
        MissingIngredientId = missingIngredientId;
    }

    /// <summary>How the write ended.</summary>
    public ProductOutcome Outcome { get; }

    /// <summary>The priced product, when the write succeeded.</summary>
    public ProductResponse? Product { get; }

    /// <summary>The ingredient a line asked for and the pantry does not have.</summary>
    public int MissingIngredientId { get; }

    /// <summary>The product was stored, or priced.</summary>
    public static ProductResult Priced(ProductResponse product) =>
        new(ProductOutcome.Priced, product, 0);

    /// <summary>No product carries that identifier.</summary>
    public static ProductResult ProductNotFound() =>
        new(ProductOutcome.ProductNotFound, null, 0);

    /// <summary>A line names an ingredient that does not exist.</summary>
    public static ProductResult IngredientNotFound(int ingredientId) =>
        new(ProductOutcome.IngredientNotFound, null, ingredientId);

    /// <summary>The caller was working from a copy someone else has replaced.</summary>
    public static ProductResult Stale() =>
        new(ProductOutcome.Stale, null, 0);
}
