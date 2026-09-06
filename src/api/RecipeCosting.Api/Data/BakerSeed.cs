using RecipeCosting.Api.Models.Entities;

namespace RecipeCosting.Api.Data;

/// <summary>
/// The routine a bakery starts with, so that a product has an hourly cost to
/// be charged by from the first read.
/// </summary>
public static class BakerSeed
{
    /// <summary>The single row written by the migration.</summary>
    public static readonly BakerSettings Row = new()
    {
        Id = BakerSettings.SingleRowId,
        MonthlyIncome = 4000m,
        HoursPerDay = 6m,
        DaysPerWeek = 5m,
        MonthlyFixedCosts = 1250m,
        DefaultMarkup = 100m,
        CardFee = 3.5m,
        Tax = 6m,
    };
}
