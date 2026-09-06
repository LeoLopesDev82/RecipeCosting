namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// What one hour of the baker's work has to pay for.
/// </summary>
public class HourlyCostResponse
{
    /// <summary>Weeks counted in a month when turning the routine into hours.</summary>
    public decimal WeeksPerMonth { get; set; }

    /// <summary>Hours the routine leaves in a month.</summary>
    public decimal HoursPerMonth { get; set; }

    /// <summary>Share of the wanted income that falls on one hour.</summary>
    public decimal Labour { get; set; }

    /// <summary>Share of the fixed costs that falls on one hour.</summary>
    public decimal Overhead { get; set; }

    /// <summary>Labour and overhead together: what an hour costs.</summary>
    public decimal Total { get; set; }
}
