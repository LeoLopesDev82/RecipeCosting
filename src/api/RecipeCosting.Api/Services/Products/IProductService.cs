using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Models.Results;

namespace RecipeCosting.Api.Services.Products;

/// <summary>
/// Reads and writes the products the baker sells, pricing each one from the
/// ingredients it uses and the time it takes.
/// </summary>
public interface IProductService
{
    /// <summary>Lists every product, priced, in alphabetical order.</summary>
    Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Finds one product, priced, or null when it does not exist.</summary>
    Task<ProductResponse?> FindAsync(int id, CancellationToken cancellationToken);

    /// <summary>Stores a new product with its recipe.</summary>
    Task<ProductResult> CreateAsync(ProductRequest request, CancellationToken cancellationToken);

    /// <summary>Replaces a product and the whole of its recipe.</summary>
    Task<ProductResult> UpdateAsync(int id, ProductRequest request, CancellationToken cancellationToken);

    /// <summary>Removes a product and the recipe behind it.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>Prices a product that is not stored, so a form can show the result as it is filled in.</summary>
    Task<ProductResult> PreviewAsync(ProductRequest request, CancellationToken cancellationToken);
}
