using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Helpers;

/// <summary>
/// Turns a wanted monthly income and a working routine into the cost of one
/// hour of work, which every product is charged by its preparation time.
/// </summary>
public static class HourlyCostHelper
{
    /// <summary>Weeks counted in a month, so that a weekly routine becomes a monthly one.</summary>
    public const decimal WeeksPerMonth = 4.33m;

    /// <summary>
    /// Works out the hours available in a month and what each one has to pay for.
    /// </summary>
    /// <param name="settings">The baker's income, routine and fixed costs.</param>
    /// <returns>Hours per month, and the labour, overhead and total cost of one hour.</returns>
    public static HourlyCostResponse Of(BakerSettings settings)
    {
        var hoursPerMonth = settings.HoursPerDay * settings.DaysPerWeek * WeeksPerMonth;
        var labour = Divide(settings.MonthlyIncome, hoursPerMonth);
        var overhead = Divide(settings.MonthlyFixedCosts, hoursPerMonth);

        return new HourlyCostResponse
        {
            WeeksPerMonth = WeeksPerMonth,
            HoursPerMonth = Round(hoursPerMonth),
            Labour = Round(labour),
            Overhead = Round(overhead),
            Total = Round(labour + overhead),
        };
    }

    private static decimal Divide(decimal amount, decimal hours)
    {
        return hours <= 0 ? 0m : amount / hours;
    }

    private static decimal Round(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
