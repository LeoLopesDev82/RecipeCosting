using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeCosting.Api.Models.Entities;

/// <summary>
/// Something the baker sells. Its cost is never stored: it is worked out from
/// the recipe lines and the price the ingredients carry today.
/// </summary>
[Table("products")]
public class Product
{
    /// <summary>Identifier of the product.</summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>Name shown to the baker, such as "Brigadeiro".</summary>
    [Required]
    [MaxLength(120)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Minutes of work one of these takes, which buys the labour cost.</summary>
    [Column("prep_minutes", TypeName = "numeric(8,2)")]
    public decimal PrepMinutes { get; set; }

    /// <summary>
    /// Markup this product prices with. Null means it follows the default the
    /// baker set, so raising that default raises this product too.
    /// </summary>
    [Column("markup", TypeName = "numeric(6,2)")]
    public decimal? Markup { get; set; }

    /// <summary>The ingredients that go into one of these, and how much of each.</summary>
    public List<ProductLine> Lines { get; set; } = [];
}
