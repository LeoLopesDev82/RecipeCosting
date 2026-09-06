using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Enums;

namespace RecipeCosting.Api.Data;

/// <summary>
/// The ingredients a bakery starts with, so that the database is useful the
/// moment it is created.
/// </summary>
public static class IngredientSeed
{
    /// <summary>
    /// The rows written by the first migration.
    /// </summary>
    public static readonly Ingredient[] Rows =
    [
        new() { Id = 1, Name = "Condensed milk", PackageSize = 395m, PackageUnit = PackageUnit.Gram, PackagePrice = 7.49m },
        new() { Id = 2, Name = "Table cream", PackageSize = 200m, PackageUnit = PackageUnit.Gram, PackagePrice = 4.29m },
        new() { Id = 3, Name = "Whole milk", PackageSize = 1000m, PackageUnit = PackageUnit.Millilitre, PackagePrice = 5.49m },
        new() { Id = 4, Name = "Cream cheese", PackageSize = 150m, PackageUnit = PackageUnit.Gram, PackagePrice = 9.9m },
        new() { Id = 5, Name = "Whipping cream", PackageSize = 1000m, PackageUnit = PackageUnit.Millilitre, PackagePrice = 28.9m },
        new() { Id = 6, Name = "Unsalted butter", PackageSize = 200m, PackageUnit = PackageUnit.Gram, PackagePrice = 12.9m },
        new() { Id = 7, Name = "Baking margarine", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 9.9m },
        new() { Id = 8, Name = "Vegetable oil", PackageSize = 900m, PackageUnit = PackageUnit.Millilitre, PackagePrice = 8.49m },
        new() { Id = 9, Name = "All-purpose flour", PackageSize = 1000m, PackageUnit = PackageUnit.Gram, PackagePrice = 5.79m },
        new() { Id = 10, Name = "Cornstarch", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 6.49m },
        new() { Id = 11, Name = "Cornmeal", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 4.19m },
        new() { Id = 12, Name = "Granulated sugar", PackageSize = 1000m, PackageUnit = PackageUnit.Gram, PackagePrice = 4.89m },
        new() { Id = 13, Name = "Icing sugar", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 5.29m },
        new() { Id = 14, Name = "Brown sugar", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 8.9m },
        new() { Id = 15, Name = "Glucose syrup", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 12.5m },
        new() { Id = 16, Name = "Honey", PackageSize = 250m, PackageUnit = PackageUnit.Gram, PackagePrice = 18.9m },
        new() { Id = 17, Name = "Cocoa powder", PackageSize = 200m, PackageUnit = PackageUnit.Gram, PackagePrice = 14.9m },
        new() { Id = 18, Name = "Semisweet chocolate", PackageSize = 1000m, PackageUnit = PackageUnit.Gram, PackagePrice = 42.9m },
        new() { Id = 19, Name = "Milk chocolate", PackageSize = 1000m, PackageUnit = PackageUnit.Gram, PackagePrice = 44.9m },
        new() { Id = 20, Name = "White chocolate", PackageSize = 1000m, PackageUnit = PackageUnit.Gram, PackagePrice = 46.5m },
        new() { Id = 21, Name = "Roasted peanuts", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 14.9m },
        new() { Id = 22, Name = "Walnut halves", PackageSize = 100m, PackageUnit = PackageUnit.Gram, PackagePrice = 19.9m },
        new() { Id = 23, Name = "Grated coconut", PackageSize = 100m, PackageUnit = PackageUnit.Gram, PackagePrice = 6.9m },
        new() { Id = 24, Name = "Strawberries", PackageSize = 300m, PackageUnit = PackageUnit.Gram, PackagePrice = 12.9m },
        new() { Id = 25, Name = "Vanilla extract", PackageSize = 100m, PackageUnit = PackageUnit.Millilitre, PackagePrice = 18.9m },
        new() { Id = 26, Name = "Red food colouring", PackageSize = 30m, PackageUnit = PackageUnit.Millilitre, PackagePrice = 7.9m },
        new() { Id = 27, Name = "Lemon", PackageSize = 12m, PackageUnit = PackageUnit.Unit, PackagePrice = 9.6m },
        new() { Id = 28, Name = "Chocolate sprinkles", PackageSize = 500m, PackageUnit = PackageUnit.Gram, PackagePrice = 13.9m },
        new() { Id = 29, Name = "Confetti sprinkles", PackageSize = 100m, PackageUnit = PackageUnit.Gram, PackagePrice = 8.4m },
        new() { Id = 30, Name = "Eggs", PackageSize = 30m, PackageUnit = PackageUnit.Unit, PackagePrice = 24.9m },
        new() { Id = 31, Name = "Baking powder", PackageSize = 100m, PackageUnit = PackageUnit.Gram, PackagePrice = 4.19m },
        new() { Id = 32, Name = "Unflavoured gelatin", PackageSize = 24m, PackageUnit = PackageUnit.Gram, PackagePrice = 5.9m },
        new() { Id = 33, Name = "Fine salt", PackageSize = 1000m, PackageUnit = PackageUnit.Gram, PackagePrice = 3.29m },
        new() { Id = 34, Name = "Cupcake liners", PackageSize = 100m, PackageUnit = PackageUnit.Unit, PackagePrice = 9.9m },
        new() { Id = 35, Name = "Cake box 20 cm", PackageSize = 10m, PackageUnit = PackageUnit.Unit, PackagePrice = 24.0m },
    ];
}
