using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeCosting.Api.Models.Entities;

/// <summary>
/// What the baker wants to earn, how long they work for it, and the margin
/// they price with. One row: the bakery has a single set of numbers.
/// </summary>
[Table("baker_settings")]
public class BakerSettings
{
    /// <summary>Identifier of the only row this table holds.</summary>
    public const int SingleRowId = 1;

    /// <summary>Identifier of the row.</summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>What the baker wants to take home in a month.</summary>
    [Column("monthly_income", TypeName = "numeric(12,2)")]
    public decimal MonthlyIncome { get; set; }

    /// <summary>Hours worked on a working day.</summary>
    [Column("hours_per_day", TypeName = "numeric(5,2)")]
    public decimal HoursPerDay { get; set; }

    /// <summary>Working days in a week.</summary>
    [Column("days_per_week", TypeName = "numeric(5,2)")]
    public decimal DaysPerWeek { get; set; }

    /// <summary>What the month costs even when nothing is baked.</summary>
    [Column("monthly_fixed_costs", TypeName = "numeric(12,2)")]
    public decimal MonthlyFixedCosts { get; set; }

    /// <summary>Markup applied to a product that does not carry its own.</summary>
    [Column("default_markup", TypeName = "numeric(6,2)")]
    public decimal DefaultMarkup { get; set; }

    /// <summary>Share of the selling price taken by the card operator.</summary>
    [Column("card_fee", TypeName = "numeric(6,2)")]
    public decimal CardFee { get; set; }

    /// <summary>Share of the selling price taken as tax.</summary>
    [Column("tax", TypeName = "numeric(6,2)")]
    public decimal Tax { get; set; }
}
