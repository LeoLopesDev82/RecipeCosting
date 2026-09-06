using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Enums;

namespace RecipeCosting.Api.Tests;

public class UnitCostHelperTests
{
    [Fact]
    public void A_package_measured_in_grams_is_costed_by_the_kilogram()
    {
        var condensedMilk = Package(395m, PackageUnit.Gram, 7.49m);

        var cost = UnitCostHelper.Of(condensedMilk);

        Assert.Equal(18.96m, cost.Amount);
        Assert.Equal("kg", cost.Label);
    }

    [Fact]
    public void A_package_measured_in_millilitres_is_costed_by_the_litre()
    {
        var oil = Package(900m, PackageUnit.Millilitre, 8.49m);

        var cost = UnitCostHelper.Of(oil);

        Assert.Equal(9.43m, cost.Amount);
        Assert.Equal("L", cost.Label);
    }

    [Fact]
    public void A_package_of_pieces_is_costed_by_the_piece()
    {
        var eggs = Package(30m, PackageUnit.Unit, 24.90m);

        var cost = UnitCostHelper.Of(eggs);

        Assert.Equal(0.83m, cost.Amount);
        Assert.Equal("unit", cost.Label);
    }

    [Fact]
    public void A_package_that_holds_nothing_costs_nothing_rather_than_dividing_by_zero()
    {
        var nothing = Package(0m, PackageUnit.Gram, 7.49m);

        var cost = UnitCostHelper.Of(nothing);

        Assert.Equal(0m, cost.Amount);
    }

    [Fact]
    public void Half_a_cent_rounds_away_from_zero_so_the_baker_is_not_short()
    {
        var awkward = Package(2m, PackageUnit.Unit, 0.05m);

        var cost = UnitCostHelper.Of(awkward);

        Assert.Equal(0.03m, cost.Amount);
    }

    [Fact]
    public void Using_part_of_a_package_costs_that_part_of_its_price()
    {
        var condensedMilk = Package(395m, PackageUnit.Gram, 7.49m);

        Assert.Equal(0.34m, UnitCostHelper.CostOfUse(condensedMilk, 18m));
    }

    [Fact]
    public void A_fraction_of_a_piece_is_allowed_because_a_recipe_can_use_half_an_egg()
    {
        var eggs = Package(30m, PackageUnit.Unit, 24.90m);

        Assert.Equal(0.42m, UnitCostHelper.CostOfUse(eggs, 0.5m));
    }

    [Fact]
    public void Using_the_whole_package_costs_the_whole_price()
    {
        var flour = Package(1000m, PackageUnit.Gram, 5.79m);

        Assert.Equal(5.79m, UnitCostHelper.CostOfUse(flour, 1000m));
    }

    private static Ingredient Package(decimal size, PackageUnit unit, decimal price)
    {
        return new Ingredient
        {
            Id = 1,
            Name = "Ingredient",
            PackageSize = size,
            PackageUnit = unit,
            PackagePrice = price,
        };
    }
}
