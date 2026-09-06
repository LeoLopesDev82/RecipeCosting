using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Enums;

namespace RecipeCosting.Api.Tests;

public class ProductCostHelperTests
{
    private static readonly Ingredient CondensedMilk = new()
    {
        Id = 1,
        Name = "Condensed milk",
        PackageSize = 395m,
        PackageUnit = PackageUnit.Gram,
        PackagePrice = 7.49m,
    };

    private static readonly Ingredient CocoaPowder = new()
    {
        Id = 2,
        Name = "Cocoa powder",
        PackageSize = 200m,
        PackageUnit = PackageUnit.Gram,
        PackagePrice = 14.90m,
    };

    private static readonly Dictionary<int, Ingredient> Pantry = new()
    {
        [CondensedMilk.Id] = CondensedMilk,
        [CocoaPowder.Id] = CocoaPowder,
    };

    [Fact]
    public void Each_line_is_priced_at_what_its_ingredient_costs_today()
    {
        var brigadeiro = Recipe([(CondensedMilk.Id, 18m), (CocoaPowder.Id, 2m)]);

        var lines = ProductCostHelper.LinesOf(brigadeiro, Pantry);

        Assert.Equal(2, lines.Count);
        Assert.Equal("Condensed milk", lines[0].IngredientName);
        Assert.Equal(0.34m, lines[0].Cost);
        Assert.Equal(PackageUnit.Gram, lines[0].Unit);
        Assert.Equal(0.15m, lines[1].Cost);
    }

    [Fact]
    public void A_line_whose_ingredient_is_gone_is_left_out_rather_than_priced_at_nothing()
    {
        var recipe = Recipe([(CondensedMilk.Id, 18m), (999, 5m)]);

        var lines = ProductCostHelper.LinesOf(recipe, Pantry);

        Assert.Single(lines);
        Assert.Equal(CondensedMilk.Id, lines[0].IngredientId);
    }

    [Fact]
    public void The_ingredients_add_up_to_what_the_lines_show()
    {
        var brigadeiro = Recipe([(CondensedMilk.Id, 18m), (CocoaPowder.Id, 2m)]);
        var lines = ProductCostHelper.LinesOf(brigadeiro, Pantry);

        var cost = ProductCostHelper.Of(brigadeiro, lines, Bakery());

        Assert.Equal(lines.Sum(line => line.Cost), cost.Ingredients);
        Assert.Equal(0.49m, cost.Ingredients);
    }

    [Fact]
    public void The_preparation_time_is_charged_at_the_cost_of_an_hour()
    {
        var brigadeiro = Recipe([(CondensedMilk.Id, 18m)], prepMinutes: 30m);
        var lines = ProductCostHelper.LinesOf(brigadeiro, Pantry);

        var cost = ProductCostHelper.Of(brigadeiro, lines, Bakery());

        Assert.Equal(20.21m, cost.Labour);
        Assert.Equal(cost.Ingredients + cost.Labour, cost.Total);
    }

    [Fact]
    public void A_product_without_a_markup_of_its_own_follows_the_bakery()
    {
        var brigadeiro = Recipe([(CondensedMilk.Id, 18m)], markup: null);
        var lines = ProductCostHelper.LinesOf(brigadeiro, Pantry);

        var cost = ProductCostHelper.Of(brigadeiro, lines, Bakery(defaultMarkup: 120m));

        Assert.Equal(120m, cost.Markup);
        Assert.True(cost.Inherited);
    }

    [Fact]
    public void A_product_with_its_own_markup_ignores_the_bakery()
    {
        var weddingCake = Recipe([(CondensedMilk.Id, 18m)], markup: 150m);
        var lines = ProductCostHelper.LinesOf(weddingCake, Pantry);

        var cost = ProductCostHelper.Of(weddingCake, lines, Bakery(defaultMarkup: 100m));

        Assert.Equal(150m, cost.Markup);
        Assert.False(cost.Inherited);
    }

    [Fact]
    public void The_markup_goes_on_top_of_the_cost()
    {
        var product = Recipe([(CondensedMilk.Id, 18m)], prepMinutes: 0m, markup: 100m);
        var lines = ProductCostHelper.LinesOf(product, Pantry);

        var cost = ProductCostHelper.Of(product, lines, Bakery(cardFee: 0m, tax: 0m));

        Assert.Equal(0.34m, cost.Total);
        Assert.Equal(0.68m, cost.Price);
    }

    [Fact]
    public void The_fees_are_added_back_because_they_come_off_the_selling_price()
    {
        var product = Recipe([(CondensedMilk.Id, 18m)], prepMinutes: 0m, markup: 100m);
        var lines = ProductCostHelper.LinesOf(product, Pantry);

        var withoutFees = ProductCostHelper.Of(product, lines, Bakery(cardFee: 0m, tax: 0m));
        var withFees = ProductCostHelper.Of(product, lines, Bakery(cardFee: 3.5m, tax: 6m));

        Assert.Equal(0.68m, withoutFees.Price);
        Assert.Equal(0.75m, withFees.Price);
        Assert.Equal(withFees.Price - withFees.Price * 0.095m, withoutFees.Price, 1);
    }

    [Fact]
    public void Fees_that_would_take_the_whole_price_leave_no_price_at_all()
    {
        var product = Recipe([(CondensedMilk.Id, 18m)]);
        var lines = ProductCostHelper.LinesOf(product, Pantry);

        var cost = ProductCostHelper.Of(product, lines, Bakery(cardFee: 60m, tax: 40m));

        Assert.Equal(0m, cost.Price);
    }

    [Fact]
    public void A_recipe_with_no_lines_still_costs_the_time_it_takes()
    {
        var product = Recipe([], prepMinutes: 30m);
        var lines = ProductCostHelper.LinesOf(product, Pantry);

        var cost = ProductCostHelper.Of(product, lines, Bakery());

        Assert.Empty(lines);
        Assert.Equal(0m, cost.Ingredients);
        Assert.Equal(20.21m, cost.Labour);
    }

    private static Product Recipe(
        (int IngredientId, decimal Quantity)[] lines,
        decimal prepMinutes = 2m,
        decimal? markup = null)
    {
        return new Product
        {
            Id = 1,
            Name = "Product",
            PrepMinutes = prepMinutes,
            Markup = markup,
            Lines = lines
                .Select(line => new ProductLine
                {
                    IngredientId = line.IngredientId,
                    Quantity = line.Quantity,
                })
                .ToList(),
        };
    }

    private static BakerSettings Bakery(
        decimal defaultMarkup = 100m,
        decimal cardFee = 3.5m,
        decimal tax = 6m)
    {
        return new BakerSettings
        {
            Id = BakerSettings.SingleRowId,
            MonthlyIncome = 4000m,
            HoursPerDay = 6m,
            DaysPerWeek = 5m,
            MonthlyFixedCosts = 1250m,
            DefaultMarkup = defaultMarkup,
            CardFee = cardFee,
            Tax = tax,
        };
    }
}
