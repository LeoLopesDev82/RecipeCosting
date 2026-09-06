using Microsoft.EntityFrameworkCore;
using RecipeCosting.Api.Data;
using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Models.Results;

namespace RecipeCosting.Api.Services.Products;

/// <inheritdoc cref="IProductService"/>
public class ProductService : IProductService
{
    private readonly RecipeCostingDbContext _context;

    public ProductService(RecipeCostingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProductResponse>> ListAsync(CancellationToken cancellationToken)
    {
        var products = await Query()
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);

        var pantry = await PantryAsync(cancellationToken);
        var settings = await SettingsAsync(cancellationToken);

        return products.Select(product => ToResponse(product, pantry, settings)).ToList();
    }

    public async Task<ProductResponse?> FindAsync(int id, CancellationToken cancellationToken)
    {
        var product = await Query().FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (product == null)
            return null;

        return ToResponse(product, await PantryAsync(cancellationToken), await SettingsAsync(cancellationToken));
    }

    public async Task<ProductResult> CreateAsync(ProductRequest request, CancellationToken cancellationToken)
    {
        var pantry = await PantryAsync(cancellationToken);
        var missing = FirstMissingIngredient(request, pantry);

        if (missing != null)
            return ProductResult.IngredientNotFound(missing.Value);

        var product = new Product();

        Apply(request, product);

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ProductResult.Priced(ToResponse(product, pantry, await SettingsAsync(cancellationToken)));
    }

    public async Task<ProductResult> UpdateAsync(int id, ProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(candidate => candidate.Lines)
            .FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (product == null)
            return ProductResult.ProductNotFound();

        var pantry = await PantryAsync(cancellationToken);
        var missing = FirstMissingIngredient(request, pantry);

        if (missing != null)
            return ProductResult.IngredientNotFound(missing.Value);

        _context.ProductLines.RemoveRange(product.Lines);

        Apply(request, product);

        await _context.SaveChangesAsync(cancellationToken);

        return ProductResult.Priced(ToResponse(product, pantry, await SettingsAsync(cancellationToken)));
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(candidate => candidate.Lines)
            .FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (product == null)
            return false;

        _context.Products.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ProductResult> PreviewAsync(ProductRequest request, CancellationToken cancellationToken)
    {
        var pantry = await PantryAsync(cancellationToken);
        var missing = FirstMissingIngredient(request, pantry);

        if (missing != null)
            return ProductResult.IngredientNotFound(missing.Value);

        var product = new Product();

        Apply(request, product);

        return ProductResult.Priced(ToResponse(product, pantry, await SettingsAsync(cancellationToken)));
    }

    #region Private methods

    private IQueryable<Product> Query()
    {
        return _context.Products.AsNoTracking().Include(product => product.Lines);
    }

    private async Task<Dictionary<int, Ingredient>> PantryAsync(CancellationToken cancellationToken)
    {
        return await _context.Ingredients
            .AsNoTracking()
            .ToDictionaryAsync(ingredient => ingredient.Id, cancellationToken);
    }

    private async Task<BakerSettings> SettingsAsync(CancellationToken cancellationToken)
    {
        return await _context.BakerSettings
            .AsNoTracking()
            .FirstAsync(row => row.Id == BakerSettings.SingleRowId, cancellationToken);
    }

    private static int? FirstMissingIngredient(ProductRequest request, Dictionary<int, Ingredient> pantry)
    {
        return request.Lines
            .Select(line => line.IngredientId)
            .Where(id => !pantry.ContainsKey(id))
            .Select(id => (int?)id)
            .FirstOrDefault();
    }

    private static void Apply(ProductRequest request, Product product)
    {
        product.Name = request.Name.Trim();
        product.PrepMinutes = request.PrepMinutes;
        product.Markup = request.Markup;
        product.Lines = request.Lines
            .Select(line => new ProductLine { IngredientId = line.IngredientId, Quantity = line.Quantity })
            .ToList();
    }

    private static ProductResponse ToResponse(
        Product product,
        IReadOnlyDictionary<int, Ingredient> pantry,
        BakerSettings settings)
    {
        var lines = ProductCostHelper.LinesOf(product, pantry);

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            PrepMinutes = product.PrepMinutes,
            Markup = product.Markup,
            Lines = lines,
            Cost = ProductCostHelper.Of(product, lines, settings),
        };
    }

    #endregion
}
