using RecipeCosting.Api.Models.Enums;

namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// An ingredient as the client sees it, with its unit cost already worked out.
/// </summary>
public class IngredientResponse
{
    /// <summary>Identifier of the ingredient.</summary>
    public int Id { get; set; }

    /// <summary>Name shown to the baker.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>How much the package holds.</summary>
    public decimal PackageSize { get; set; }

    /// <summary>The unit the package is measured in.</summary>
    public PackageUnit PackageUnit { get; set; }

    /// <summary>What the whole package costs.</summary>
    public decimal PackagePrice { get; set; }

    /// <summary>The package price normalised per kilogram, litre or piece.</summary>
    public UnitCostResponse UnitCost { get; set; } = new();
}
