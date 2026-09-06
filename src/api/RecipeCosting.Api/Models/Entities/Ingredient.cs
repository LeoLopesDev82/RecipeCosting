using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecipeCosting.Api.Models.Enums;

namespace RecipeCosting.Api.Models.Entities;

/// <summary>
/// Something the baker buys, priced by the package it comes in.
/// </summary>
[Table("ingredients")]
public class Ingredient
{
    /// <summary>Identifier of the ingredient.</summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>Name shown to the baker, such as "Condensed milk".</summary>
    [Required]
    [MaxLength(120)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>How much the package holds, in the unit below.</summary>
    [Column("package_size", TypeName = "numeric(12,2)")]
    public decimal PackageSize { get; set; }

    /// <summary>The unit the package is measured in.</summary>
    [Column("package_unit")]
    public PackageUnit PackageUnit { get; set; }

    /// <summary>What the whole package costs.</summary>
    [Column("package_price", TypeName = "numeric(12,2)")]
    public decimal PackagePrice { get; set; }
}
