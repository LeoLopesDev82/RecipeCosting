using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Models.Entities;

namespace RecipeCosting.Api.Tests;

public class HourlyCostHelperTests
{
    [Fact]
    public void The_routine_becomes_the_hours_a_month_holds()
    {
        var cost = HourlyCostHelper.Of(Routine(hoursPerDay: 6m, daysPerWeek: 5m));

        Assert.Equal(129.90m, cost.HoursPerMonth);
        Assert.Equal(HourlyCostHelper.WeeksPerMonth, cost.WeeksPerMonth);
    }

    [Fact]
    public void The_wanted_income_is_spread_over_those_hours()
    {
        var cost = HourlyCostHelper.Of(Routine(monthlyIncome: 4000m));

        Assert.Equal(30.79m, cost.Labour);
    }

    [Fact]
    public void The_fixed_costs_are_spread_over_those_hours_too()
    {
        var cost = HourlyCostHelper.Of(Routine(monthlyFixedCosts: 1250m));

        Assert.Equal(9.62m, cost.Overhead);
    }

    [Fact]
    public void The_total_is_rounded_once_at_the_end_and_not_summed_from_rounded_parts()
    {
        var cost = HourlyCostHelper.Of(Routine(monthlyIncome: 4000m, monthlyFixedCosts: 1250m));

        Assert.Equal(30.79m, cost.Labour);
        Assert.Equal(9.62m, cost.Overhead);
        Assert.Equal(40.42m, cost.Total);
        Assert.NotEqual(cost.Labour + cost.Overhead, cost.Total);
    }

    [Fact]
    public void Working_longer_for_the_same_income_makes_an_hour_cheaper()
    {
        var shortDay = HourlyCostHelper.Of(Routine(hoursPerDay: 6m));
        var longDay = HourlyCostHelper.Of(Routine(hoursPerDay: 8m));

        Assert.True(longDay.Labour < shortDay.Labour);
        Assert.True(longDay.Overhead < shortDay.Overhead);
    }

    [Fact]
    public void A_routine_of_no_hours_costs_nothing_rather_than_dividing_by_zero()
    {
        var cost = HourlyCostHelper.Of(Routine(hoursPerDay: 0m, daysPerWeek: 0m));

        Assert.Equal(0m, cost.HoursPerMonth);
        Assert.Equal(0m, cost.Labour);
        Assert.Equal(0m, cost.Overhead);
        Assert.Equal(0m, cost.Total);
    }

    [Fact]
    public void A_baker_who_wants_nothing_and_pays_nothing_still_has_hours()
    {
        var cost = HourlyCostHelper.Of(Routine(monthlyIncome: 0m, monthlyFixedCosts: 0m));

        Assert.Equal(129.90m, cost.HoursPerMonth);
        Assert.Equal(0m, cost.Total);
    }

    private static BakerSettings Routine(
        decimal monthlyIncome = 4000m,
        decimal hoursPerDay = 6m,
        decimal daysPerWeek = 5m,
        decimal monthlyFixedCosts = 1250m)
    {
        return new BakerSettings
        {
            Id = BakerSettings.SingleRowId,
            MonthlyIncome = monthlyIncome,
            HoursPerDay = hoursPerDay,
            DaysPerWeek = daysPerWeek,
            MonthlyFixedCosts = monthlyFixedCosts,
            DefaultMarkup = 100m,
            CardFee = 3.5m,
            Tax = 6m,
        };
    }
}
