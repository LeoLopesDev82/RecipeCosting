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

    /// <summary>Minutes of work the whole recipe takes, which buys the labour cost.</summary>
    [Column("prep_minutes", TypeName = "numeric(8,2)")]
    public decimal PrepMinutes { get; set; }

    /// <summary>
    /// How many units the recipe makes. One means the recipe is already a single
    /// item, so the cost of the batch and the cost of a unit are the same figure.
    /// </summary>
    [Column("yield")]
    public int Yield { get; set; } = 1;

    /// <summary>
    /// Markup this product prices with. Null means it follows the default the
    /// baker set, so raising that default raises this product too.
    /// </summary>
    [Column("markup", TypeName = "numeric(6,2)")]
    public decimal? Markup { get; set; }

    /// <summary>The ingredients the whole recipe uses, and how much of each.</summary>
    public List<ProductLine> Lines { get; set; } = [];

    /// <summary>
    /// Rises by one on every write. A save that carries an older number is refused,
    /// so two people editing the same record do not overwrite each other in silence.
    /// </summary>
    [ConcurrencyCheck]
    [Column("version")]
    public int Version { get; set; }
}
