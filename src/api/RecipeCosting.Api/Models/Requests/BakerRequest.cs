using System.ComponentModel.DataAnnotations;

namespace RecipeCosting.Api.Models.Requests;

/// <summary>
/// The baker's numbers as the client sends them. The hourly cost is never
/// sent: the API works it out from the routine.
/// </summary>
public class BakerRequest : IValidatableObject
{
    /// <summary>What the baker wants to take home in a month.</summary>
    [Range(0.01, 1_000_000, ErrorMessage = "The monthly income must be greater than zero.")]
    public decimal MonthlyIncome { get; set; }

    /// <summary>Hours worked on a working day.</summary>
    [Range(0.5, 24, ErrorMessage = "The hours per day must be between 0.5 and 24.")]
    public decimal HoursPerDay { get; set; }

    /// <summary>Working days in a week.</summary>
    [Range(1, 7, ErrorMessage = "The days per week must be between 1 and 7.")]
    public decimal DaysPerWeek { get; set; }

    /// <summary>What the month costs even when nothing is baked.</summary>
    [Range(0, 1_000_000, ErrorMessage = "The monthly fixed costs cannot be negative.")]
    public decimal MonthlyFixedCosts { get; set; }

    /// <summary>Markup applied to a product that does not carry its own.</summary>
    [Range(0, 1000, ErrorMessage = "The default markup must be between 0 and 1000.")]
    public decimal DefaultMarkup { get; set; }

    /// <summary>Share of the selling price taken by the card operator.</summary>
    [Range(0, 100, ErrorMessage = "The card fee must be between 0 and 100.")]
    public decimal CardFee { get; set; }

    /// <summary>Share of the selling price taken as tax.</summary>
    [Range(0, 100, ErrorMessage = "The tax must be between 0 and 100.")]
    public decimal Tax { get; set; }

    /// <summary>
    /// Fees come off the selling price, so together they have to leave something
    /// behind. At a hundred per cent there is no price that pays the cost.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CardFee + Tax >= 100)
            yield return new ValidationResult(
                "The card fee and the tax cannot take the whole selling price.",
                [nameof(CardFee), nameof(Tax)]);
    }
}
