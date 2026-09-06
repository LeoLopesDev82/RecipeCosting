using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RecipeCosting.Api.Data;

namespace RecipeCosting.Api.Tests;

/// <summary>
/// Starts the real application over an in-memory database seeded by the same
/// migrations data the real one is created from.
/// </summary>
public class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _database = $"recipe-costing-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Auth:SigningKey", "a-signing-key-long-enough-for-the-tests");

        builder.ConfigureServices(services =>
        {
            Forget(services);

            services.AddDbContext<RecipeCostingDbContext>(options =>
                options.UseInMemoryDatabase(_database));
        });
    }

    /// <summary>
    /// A client carrying a token, which every endpoint but the login demands.
    /// </summary>
    public async Task<HttpClient> SignedInAsync()
    {
        var client = CreateClient();

        Seed();

        var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "demo@recipecosting.local", password = "demo1234" });

        var session = await response.Content.ReadFromJsonAsync<Session>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", session!.AccessToken);

        return client;
    }

    /// <summary>
    /// Drops everything the PostgreSQL registration left behind. Entity Framework
    /// refuses to hold two providers at once, and a database context brings more
    /// than its options along.
    /// </summary>
    private static void Forget(IServiceCollection services)
    {
        var registrations = services
            .Where(service =>
                service.ServiceType == typeof(RecipeCostingDbContext)
                || service.ServiceType.FullName?.Contains("DbContextOptions") == true)
            .ToList();

        registrations.ForEach(service => services.Remove(service));
    }

    private void Seed()
    {
        using var scope = Services.CreateScope();

        scope.ServiceProvider.GetRequiredService<RecipeCostingDbContext>().Database.EnsureCreated();
    }

    private sealed record Session(string AccessToken);
}
