using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Services.Baker;

/// <summary>
/// Reads and writes the single set of numbers the bakery prices by.
/// </summary>
public interface IBakerService
{
    /// <summary>Returns the baker's numbers with the hourly cost they produce.</summary>
    Task<BakerResponse> GetAsync(CancellationToken cancellationToken);

    /// <summary>Replaces the baker's numbers and returns the hourly cost they now produce.</summary>
    Task<BakerResponse> UpdateAsync(BakerRequest request, CancellationToken cancellationToken);

    /// <summary>Simulates an hourly cost without storing anything.</summary>
    HourlyCostResponse Preview(BakerRequest request);
}
