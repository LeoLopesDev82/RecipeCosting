using Microsoft.EntityFrameworkCore;
using RecipeCosting.Api.Data;
using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Models.Entities;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Services.Ingredients;

/// <inheritdoc cref="IIngredientService"/>
public class IngredientService : IIngredientService
{
    private readonly RecipeCostingDbContext _context;

    public IngredientService(RecipeCostingDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<IngredientResponse>> ListAsync(CancellationToken cancellationToken)
    {
        var ingredients = await _context.Ingredients
            .AsNoTracking()
            .OrderBy(ingredient => ingredient.Name)
            .ToListAsync(cancellationToken);

        return ingredients.Select(ToResponse).ToList();
    }

    public async Task<IngredientResponse?> FindAsync(int id, CancellationToken cancellationToken)
    {
        var ingredient = await _context.Ingredients
            .AsNoTracking()
            .FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        return ingredient == null ? null : ToResponse(ingredient);
    }

    public async Task<IngredientResponse> CreateAsync(IngredientRequest request, CancellationToken cancellationToken)
    {
        var ingredient = new Ingredient();

        Apply(request, ingredient);

        await _context.Ingredients.AddAsync(ingredient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ToResponse(ingredient);
    }

    public async Task<IngredientResponse?> UpdateAsync(int id, IngredientRequest request, CancellationToken cancellationToken)
    {
        var ingredient = await _context.Ingredients
            .FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (ingredient == null)
            return null;

        Apply(request, ingredient);

        await _context.SaveChangesAsync(cancellationToken);

        return ToResponse(ingredient);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var ingredient = await _context.Ingredients
            .FirstOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);

        if (ingredient == null)
            return false;

        _context.Ingredients.Remove(ingredient);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    #region Private methods

    private static void Apply(IngredientRequest request, Ingredient ingredient)
    {
        ingredient.Name = request.Name.Trim();
        ingredient.PackageSize = request.PackageSize;
        ingredient.PackageUnit = request.PackageUnit;
        ingredient.PackagePrice = request.PackagePrice;
    }

    private static IngredientResponse ToResponse(Ingredient ingredient)
    {
        return new IngredientResponse
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            PackageSize = ingredient.PackageSize,
            PackageUnit = ingredient.PackageUnit,
            PackagePrice = ingredient.PackagePrice,
            UnitCost = UnitCostHelper.Of(ingredient),
        };
    }

    #endregion
}
