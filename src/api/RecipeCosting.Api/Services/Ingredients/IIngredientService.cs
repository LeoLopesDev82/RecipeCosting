using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Services.Ingredients;

/// <summary>
/// Reads and writes the ingredients the baker buys.
/// </summary>
public interface IIngredientService
{
    /// <summary>Lists every ingredient, in alphabetical order.</summary>
    Task<IReadOnlyList<IngredientResponse>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Finds one ingredient, or null when it does not exist.</summary>
    Task<IngredientResponse?> FindAsync(int id, CancellationToken cancellationToken);

    /// <summary>Stores a new ingredient and returns it with its unit cost.</summary>
    Task<IngredientResponse> CreateAsync(IngredientRequest request, CancellationToken cancellationToken);

    /// <summary>Replaces an ingredient, or returns null when it does not exist.</summary>
    Task<IngredientResponse?> UpdateAsync(int id, IngredientRequest request, CancellationToken cancellationToken);

    /// <summary>Removes an ingredient, reporting whether it was there to remove.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
