using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Enums;
using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Helpers;

/// <summary>
/// Turns the price of a package into the price of a single measure.
/// </summary>
public static class UnitCostHelper
{
    private static readonly Dictionary<PackageUnit, (decimal Measure, string Label)> Bases = new()
    {
        [PackageUnit.Gram] = (1000m, "kg"),
        [PackageUnit.Millilitre] = (1000m, "L"),
        [PackageUnit.Unit] = (1m, "unit"),
    };

    /// <summary>
    /// Works out what one kilogram, litre or piece of the ingredient costs.
    /// </summary>
    /// <param name="ingredient">The ingredient, priced by its package.</param>
    /// <returns>The cost of a single measure and the name of that measure.</returns>
    public static UnitCostResponse Of(Ingredient ingredient)
    {
        var (measure, label) = Bases[ingredient.PackageUnit];

        return new UnitCostResponse
        {
            Amount = Round(ingredient.PackagePrice * measure / ingredient.PackageSize),
            Label = label,
        };
    }

    /// <summary>
    /// Works out what a given quantity of the ingredient costs inside a recipe.
    /// </summary>
    /// <param name="ingredient">The ingredient, priced by its package.</param>
    /// <param name="quantity">Quantity used, in the package unit.</param>
    /// <returns>The cost of that quantity.</returns>
    public static decimal CostOfUse(Ingredient ingredient, decimal quantity)
    {
        return Round(ingredient.PackagePrice * quantity / ingredient.PackageSize);
    }

    private static decimal Round(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
