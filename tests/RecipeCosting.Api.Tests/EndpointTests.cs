using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace RecipeCosting.Api.Tests;

/// <summary>
/// The few paths where a mistake would be expensive and a unit test cannot
/// reach: authorisation, the guard that keeps a recipe from losing an
/// ingredient, and a price travelling from the database to the response.
/// </summary>
public class EndpointTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _api;

    public EndpointTests(ApiFactory api)
    {
        _api = api;
    }

    [Fact]
    public async Task Nothing_answers_without_a_token()
    {
        var stranger = _api.CreateClient();

        Assert.Equal(HttpStatusCode.Unauthorized, (await stranger.GetAsync("/api/ingredients")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await stranger.GetAsync("/api/products")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await stranger.GetAsync("/api/baker")).StatusCode);
    }

    [Fact]
    public async Task An_ingredient_a_recipe_still_uses_is_kept_and_the_reason_given()
    {
        var client = await _api.SignedInAsync();

        var response = await client.DeleteAsync("/api/ingredients/1");
        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Contains("product", problem.GetProperty("detail").GetString()!, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/ingredients/1")).StatusCode);
    }

    [Fact]
    public async Task A_recipe_cannot_name_an_ingredient_that_is_not_in_the_pantry()
    {
        var client = await _api.SignedInAsync();

        var response = await client.PostAsJsonAsync("/api/products", new
        {
            name = "Impossible cake",
            prepMinutes = 10,
            markup = (decimal?)null,
            lines = new[] { new { ingredientId = 9999, quantity = 1m } },
        });

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("9999", problem.GetProperty("errors").GetProperty("Lines")[0].GetString()!);
    }

    [Fact]
    public async Task A_recipe_cannot_name_the_same_ingredient_on_two_lines()
    {
        var client = await _api.SignedInAsync();

        var response = await client.PostAsJsonAsync("/api/products", new
        {
            name = "Doubled cake",
            prepMinutes = 10,
            markup = (decimal?)null,
            lines = new[]
            {
                new { ingredientId = 1, quantity = 1m },
                new { ingredientId = 1, quantity = 2m },
            },
        });

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("twice", problem.GetProperty("errors").GetProperty("Lines")[0].GetString()!);
    }

    [Fact]
    public async Task A_product_arrives_priced_from_the_ingredients_and_the_hours_behind_it()
    {
        var client = await _api.SignedInAsync();

        var products = await client.GetFromJsonAsync<JsonElement>("/api/products");
        var brigadeiro = products.EnumerateArray().First(p => p.GetProperty("name").GetString() == "Brigadeiro");
        var cost = brigadeiro.GetProperty("cost");

        var ingredients = cost.GetProperty("ingredients").GetDecimal();
        var labour = cost.GetProperty("labour").GetDecimal();
        var total = cost.GetProperty("total").GetDecimal();
        var price = cost.GetProperty("price").GetDecimal();

        Assert.Equal(4, brigadeiro.GetProperty("lines").GetArrayLength());
        Assert.Equal(ingredients + labour, total);
        Assert.True(ingredients > 0, "the recipe lines were priced");
        Assert.True(labour > 0, "the preparation time was charged");
        Assert.True(price > total, "the markup and the fees were added");
        Assert.True(cost.GetProperty("inherited").GetBoolean(), "this product follows the bakery's markup");
    }

    [Fact]
    public async Task Changing_an_ingredient_reprices_every_product_that_uses_it()
    {
        var client = await _api.SignedInAsync();

        var before = await PriceOfBrigadeiroAsync(client);

        var condensedMilk = await client.GetFromJsonAsync<JsonElement>("/api/ingredients/1");

        await client.PutAsJsonAsync("/api/ingredients/1", new
        {
            name = condensedMilk.GetProperty("name").GetString(),
            packageSize = condensedMilk.GetProperty("packageSize").GetDecimal(),
            packageUnit = condensedMilk.GetProperty("packageUnit").GetString(),
            packagePrice = condensedMilk.GetProperty("packagePrice").GetDecimal() * 2,
        });

        Assert.True(await PriceOfBrigadeiroAsync(client) > before, "the product cost more once its ingredient did");
    }

    private static async Task<decimal> PriceOfBrigadeiroAsync(HttpClient client)
    {
        var products = await client.GetFromJsonAsync<JsonElement>("/api/products");

        return products.EnumerateArray()
            .First(product => product.GetProperty("name").GetString() == "Brigadeiro")
            .GetProperty("cost")
            .GetProperty("price")
            .GetDecimal();
    }
}
