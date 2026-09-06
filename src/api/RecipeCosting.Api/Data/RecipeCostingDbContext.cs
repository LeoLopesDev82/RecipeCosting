using Microsoft.EntityFrameworkCore;
using RecipeCosting.Api.Models.Entities;

namespace RecipeCosting.Api.Data;

/// <summary>
/// Entity Framework context for the Recipe Costing database.
/// </summary>
public class RecipeCostingDbContext : DbContext
{
    public RecipeCostingDbContext(DbContextOptions<RecipeCostingDbContext> options) : base(options)
    {
    }

    /// <summary>The ingredients the baker buys.</summary>
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>().HasData(IngredientSeed.Rows);
    }
}
