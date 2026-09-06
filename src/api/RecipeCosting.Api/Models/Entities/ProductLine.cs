using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeCosting.Api.Models.Entities;

/// <summary>
/// One ingredient inside a recipe, and how much of it is used, measured in the
/// unit the ingredient is bought by.
/// </summary>
[Table("product_lines")]
public class ProductLine
{
    /// <summary>Identifier of the line.</summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>The product this line belongs to.</summary>
    [Column("product_id")]
    public int ProductId { get; set; }

    /// <summary>The ingredient used.</summary>
    [Column("ingredient_id")]
    public int IngredientId { get; set; }

    /// <summary>How much is used, in the ingredient's package unit.</summary>
    [Column("quantity", TypeName = "numeric(12,3)")]
    public decimal Quantity { get; set; }

    /// <summary>Navigation back to the product.</summary>
    public Product? Product { get; set; }

    /// <summary>Navigation to the ingredient that carries the price.</summary>
    public Ingredient? Ingredient { get; set; }
}
