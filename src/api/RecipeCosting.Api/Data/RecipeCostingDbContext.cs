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

    /// <summary>The single row of settings the bakery prices by.</summary>
    public DbSet<BakerSettings> BakerSettings => Set<BakerSettings>();

    /// <summary>The products the baker sells.</summary>
    public DbSet<Product> Products => Set<Product>();

    /// <summary>The recipe lines behind those products.</summary>
    public DbSet<ProductLine> ProductLines => Set<ProductLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductLine>()
            .HasOne(line => line.Product)
            .WithMany(product => product.Lines)
            .HasForeignKey(line => line.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductLine>()
            .HasOne(line => line.Ingredient)
            .WithMany()
            .HasForeignKey(line => line.IngredientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ingredient>().HasData(IngredientSeed.Rows);
        modelBuilder.Entity<BakerSettings>().HasData(BakerSeed.Row);
        modelBuilder.Entity<Product>().HasData(ProductSeed.Rows);
        modelBuilder.Entity<ProductLine>().HasData(ProductSeed.Lines);
    }

    /// <summary>
    /// Says which version the caller was working from, so that the update finds no
    /// row when someone else has written since.
    /// </summary>
    /// <param name="entity">An entity the context is tracking.</param>
    /// <param name="version">The version the caller read.</param>
    public void ExpectVersion(object entity, int version)
    {
        Entry(entity).Property(nameof(Ingredient.Version)).OriginalValue = version;
    }
}
