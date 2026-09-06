using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Models.Results;
using RecipeCosting.Api.Services.Products;

namespace RecipeCosting.Api.Controllers;

/// <summary>
/// The products the baker sells, priced from the ingredients they use and the
/// time they take.
/// </summary>
[ApiController]
[Authorize]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Lists every product with its recipe, its cost and its selling price.
    /// </summary>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The whole list of products, priced at today's ingredient prices.</returns>
    /// <response code="200">The list, which may be empty.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> List(CancellationToken cancellationToken)
    {
        var products = await _productService.ListAsync(cancellationToken);

        return Ok(products);
    }

    /// <summary>
    /// Returns one product with its recipe and its price.
    /// </summary>
    /// <param name="id">Identifier of the product.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The product, priced.</returns>
    /// <response code="200">The product was found.</response>
    /// <response code="404">No product carries that identifier.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> Find(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.FindAsync(id, cancellationToken);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    /// <summary>
    /// Creates a product and its recipe. Neither cost nor price is accepted.
    /// </summary>
    /// <param name="request">Name, preparation time, optional markup and the recipe lines.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The product as it was stored, priced.</returns>
    /// <response code="201">The product was created.</response>
    /// <response code="400">A field is invalid, or a line names an ingredient that does not exist.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductResponse>> Create(
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _productService.CreateAsync(request, cancellationToken);

        if (result.Outcome == ProductOutcome.IngredientNotFound)
            return UnknownIngredient(result.MissingIngredientId);

        return CreatedAtAction(nameof(Find), new { id = result.Product!.Id }, result.Product);
    }

    /// <summary>
    /// Replaces a product and the whole of its recipe.
    /// </summary>
    /// <param name="id">Identifier of the product.</param>
    /// <param name="request">The new name, preparation time, markup and recipe lines.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The product as it was stored, priced.</returns>
    /// <response code="200">The product was replaced.</response>
    /// <response code="400">A field is invalid, or a line names an ingredient that does not exist.</response>
    /// <response code="404">No product carries that identifier.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> Update(
        int id,
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _productService.UpdateAsync(id, request, cancellationToken);

        if (result.Outcome == ProductOutcome.ProductNotFound)
            return NotFound();

        if (result.Outcome == ProductOutcome.IngredientNotFound)
            return UnknownIngredient(result.MissingIngredientId);

        return Ok(result.Product);
    }

    /// <summary>
    /// Deletes a product and the recipe behind it.
    /// </summary>
    /// <param name="id">Identifier of the product.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <response code="204">The product was deleted.</response>
    /// <response code="404">No product carries that identifier.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteAsync(id, cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Prices a recipe that is not stored, so a form can show the cost and the
    /// selling price while it is being filled in.
    /// </summary>
    /// <param name="request">The product being tried out.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The same shape a stored product returns, without storing anything.</returns>
    /// <response code="200">The simulated cost and price.</response>
    /// <response code="400">A field is invalid, or a line names an ingredient that does not exist.</response>
    [HttpPost("preview")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductResponse>> Preview(
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _productService.PreviewAsync(request, cancellationToken);

        if (result.Outcome == ProductOutcome.IngredientNotFound)
            return UnknownIngredient(result.MissingIngredientId);

        return Ok(result.Product);
    }

    #region Private methods

    private ActionResult UnknownIngredient(int ingredientId)
    {
        ModelState.AddModelError(nameof(ProductRequest.Lines), $"Ingredient {ingredientId} does not exist.");

        return ValidationProblem(ModelState);
    }

    #endregion
}
