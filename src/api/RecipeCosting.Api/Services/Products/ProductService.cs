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
        var costOfAnHour = HourlyCostHelper.Of(settings).Total;

        return products.Select(product => ToResponse(product, pantry, settings, costOfAnHour)).ToList();
    }

    public async Task<ProductResponse?> FindAsync(int id, CancellationToken cancellationToken)
    {
        var product = await Query().FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (product == null)
            return null;

        return await PricedAsync(product, cancellationToken);
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

        return ProductResult.Priced(await PricedAsync(product, cancellationToken, pantry));
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

        _context.ExpectVersion(product, request.Version);

        product.Version = request.Version + 1;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return ProductResult.Stale();
        }

        return ProductResult.Priced(await PricedAsync(product, cancellationToken, pantry));
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

        return ProductResult.Priced(await PricedAsync(product, cancellationToken, pantry));
    }

    #region Private methods

    private IQueryable<Product> Query()
    {
        return _context.Products
            .AsNoTracking()
            .Include(product => product.Lines.OrderBy(line => line.Id));
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
        product.Yield = request.Yield;
        product.Markup = request.Markup;
        product.Lines = request.Lines
            .Select(line => new ProductLine { IngredientId = line.IngredientId, Quantity = line.Quantity })
            .ToList();
    }

    private async Task<ProductResponse> PricedAsync(
        Product product,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<int, Ingredient>? pantry = null)
    {
        var settings = await SettingsAsync(cancellationToken);

        return ToResponse(
            product,
            pantry ?? await PantryAsync(cancellationToken),
            settings,
            HourlyCostHelper.Of(settings).Total);
    }

    private static ProductResponse ToResponse(
        Product product,
        IReadOnlyDictionary<int, Ingredient> pantry,
        BakerSettings settings,
        decimal costOfAnHour)
    {
        var lines = ProductCostHelper.LinesOf(product, pantry);

        return new ProductResponse
        {
            Id = product.Id,
            Version = product.Version,
            Name = product.Name,
            PrepMinutes = product.PrepMinutes,
            Yield = product.Yield,
            Markup = product.Markup,
            Lines = lines,
            Cost = ProductCostHelper.Of(product, lines, settings, costOfAnHour),
        };
    }

    #endregion
}
