using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Helpers;

/// <summary>
/// Prices a product: what its recipe costs today, what the time spent on it is
/// worth, and what has to be charged for it to be worth making.
/// </summary>
public static class ProductCostHelper
{
    /// <summary>
    /// Reads each recipe line at the ingredient's current price.
    /// </summary>
    /// <param name="product">The product and its lines.</param>
    /// <param name="pantry">The ingredients, by identifier.</param>
    /// <returns>One priced line per ingredient used.</returns>
    public static List<ProductLineResponse> LinesOf(Product product, IReadOnlyDictionary<int, Ingredient> pantry)
    {
        return product.Lines
            .Where(line => pantry.ContainsKey(line.IngredientId))
            .Select(line => ToLineResponse(line, pantry[line.IngredientId]))
            .ToList();
    }

    /// <summary>
    /// Adds the recipe up, charges the preparation time at the baker's hourly
    /// cost, and puts the markup and the fees on top.
    /// </summary>
    /// <param name="product">The product being priced.</param>
    /// <param name="lines">The priced recipe lines.</param>
    /// <param name="settings">The baker's routine and pricing defaults.</param>
    /// <returns>The cost breakdown and the selling price.</returns>
    public static ProductCostResponse Of(
        Product product,
        IReadOnlyList<ProductLineResponse> lines,
        BakerSettings settings)
    {
        var ingredients = lines.Sum(line => line.Cost);
        var labour = Round(product.PrepMinutes / 60m * HourlyCostHelper.Of(settings).Total);
        var total = ingredients + labour;
        var markup = product.Markup ?? settings.DefaultMarkup;

        return new ProductCostResponse
        {
            Ingredients = ingredients,
            Labour = labour,
            Total = total,
            Markup = markup,
            Inherited = product.Markup == null,
            Price = PriceOf(total, markup, settings),
        };
    }

    #region Private methods

    private static ProductLineResponse ToLineResponse(ProductLine line, Ingredient ingredient)
    {
        return new ProductLineResponse
        {
            IngredientId = ingredient.Id,
            IngredientName = ingredient.Name,
            Quantity = line.Quantity,
            Unit = ingredient.PackageUnit,
            Cost = UnitCostHelper.CostOfUse(ingredient, line.Quantity),
        };
    }

    private static decimal PriceOf(decimal total, decimal markup, BakerSettings settings)
    {
        var kept = 1 - (settings.CardFee + settings.Tax) / 100m;

        return kept <= 0 ? 0m : Round(total * (1 + markup / 100m) / kept);
    }

    private static decimal Round(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    #endregion
}
