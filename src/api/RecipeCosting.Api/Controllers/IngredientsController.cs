using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Models.Results;
using RecipeCosting.Api.Services.Ingredients;

namespace RecipeCosting.Api.Controllers;

/// <summary>
/// The ingredients the baker buys, and what each one costs per measure.
/// </summary>
[ApiController]
[Authorize]
[Route("api/ingredients")]
[Produces("application/json")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientService _ingredientService;

    public IngredientsController(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    /// <summary>
    /// Lists every ingredient, alphabetically, with the unit cost of each one.
    /// </summary>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The whole list of ingredients.</returns>
    /// <response code="200">The list, which may be empty.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<IngredientResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<IngredientResponse>>> List(CancellationToken cancellationToken)
    {
        var ingredients = await _ingredientService.ListAsync(cancellationToken);

        return Ok(ingredients);
    }

    /// <summary>
    /// Returns one ingredient by its identifier.
    /// </summary>
    /// <param name="id">Identifier of the ingredient.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The ingredient with its unit cost.</returns>
    /// <response code="200">The ingredient was found.</response>
    /// <response code="404">No ingredient carries that identifier.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IngredientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IngredientResponse>> Find(int id, CancellationToken cancellationToken)
    {
        var ingredient = await _ingredientService.FindAsync(id, cancellationToken);

        if (ingredient == null)
            return NotFound();

        return Ok(ingredient);
    }

    /// <summary>
    /// Creates an ingredient. The unit cost is worked out here, never sent in.
    /// </summary>
    /// <param name="request">Name, package size, package unit and package price.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The ingredient as it was stored.</returns>
    /// <response code="201">The ingredient was created.</response>
    /// <response code="400">The request is missing a field or carries an invalid value.</response>
    [HttpPost]
    [ProducesResponseType(typeof(IngredientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IngredientResponse>> Create(
        [FromBody] IngredientRequest request,
        CancellationToken cancellationToken)
    {
        var ingredient = await _ingredientService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Find), new { id = ingredient.Id }, ingredient);
    }

    /// <summary>
    /// Replaces an ingredient. Every product that uses it reprices on the next read.
    /// </summary>
    /// <param name="id">Identifier of the ingredient.</param>
    /// <param name="request">The new name, package size, package unit and package price.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The ingredient as it was stored.</returns>
    /// <response code="200">The ingredient was replaced.</response>
    /// <response code="400">The request is missing a field or carries an invalid value.</response>
    /// <response code="404">No ingredient carries that identifier.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(IngredientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IngredientResponse>> Update(
        int id,
        [FromBody] IngredientRequest request,
        CancellationToken cancellationToken)
    {
        var ingredient = await _ingredientService.UpdateAsync(id, request, cancellationToken);

        if (ingredient == null)
            return NotFound();

        return Ok(ingredient);
    }

    /// <summary>
    /// Works out what a package would cost per kilogram, litre or piece, without
    /// storing anything, so a form can show the result while it is filled in.
    /// </summary>
    /// <param name="request">The package being tried out.</param>
    /// <returns>The unit cost that package would carry.</returns>
    /// <response code="200">The simulated unit cost.</response>
    /// <response code="400">A field is missing or out of range.</response>
    [HttpPost("preview")]
    [ProducesResponseType(typeof(UnitCostResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UnitCostResponse> Preview([FromBody] IngredientRequest request)
    {
        return Ok(_ingredientService.Preview(request));
    }

    /// <summary>
    /// Deletes an ingredient, unless a recipe still uses it.
    /// </summary>
    /// <param name="id">Identifier of the ingredient.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <response code="204">The ingredient was deleted.</response>
    /// <response code="404">No ingredient carries that identifier.</response>
    /// <response code="409">A product still uses it, so it was kept.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var outcome = await _ingredientService.DeleteAsync(id, cancellationToken);

        if (outcome == DeletionOutcome.NotFound)
            return NotFound();

        if (outcome == DeletionOutcome.InUse)
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "The ingredient is in use.",
                Detail = "A product still lists this ingredient. Remove it from the recipes first.",
            });

        return NoContent();
    }
}
