namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// The baker's numbers, with the cost of an hour already worked out.
/// </summary>
public class BakerResponse
{
    /// <summary>The version of the record, to be sent back when replacing it.</summary>
    public int Version { get; set; }

    /// <summary>What the baker wants to take home in a month.</summary>
    public decimal MonthlyIncome { get; set; }

    /// <summary>Hours worked on a working day.</summary>
    public decimal HoursPerDay { get; set; }

    /// <summary>Working days in a week.</summary>
    public decimal DaysPerWeek { get; set; }

    /// <summary>What the month costs even when nothing is baked.</summary>
    public decimal MonthlyFixedCosts { get; set; }

    /// <summary>Markup applied to a product that does not carry its own.</summary>
    public decimal DefaultMarkup { get; set; }

    /// <summary>Share of the selling price taken by the card operator.</summary>
    public decimal CardFee { get; set; }

    /// <summary>Share of the selling price taken as tax.</summary>
    public decimal Tax { get; set; }

    /// <summary>The hourly cost these numbers produce.</summary>
    public HourlyCostResponse HourlyCost { get; set; } = new();
}
