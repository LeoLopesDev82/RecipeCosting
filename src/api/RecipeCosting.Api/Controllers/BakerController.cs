using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Services.Baker;

namespace RecipeCosting.Api.Controllers;

/// <summary>
/// What the baker wants to earn and how long they work for it, which together
/// decide what an hour of their time costs a product.
/// </summary>
[ApiController]
[Authorize]
[Route("api/baker")]
[Produces("application/json")]
public class BakerController : ControllerBase
{
    private readonly IBakerService _bakerService;

    public BakerController(IBakerService bakerService)
    {
        _bakerService = bakerService;
    }

    /// <summary>
    /// Returns the baker's numbers and the hourly cost they produce.
    /// </summary>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The stored settings with the hourly cost.</returns>
    /// <response code="200">The settings, which always exist.</response>
    [HttpGet]
    [ProducesResponseType(typeof(BakerResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<BakerResponse>> Get(CancellationToken cancellationToken)
    {
        var settings = await _bakerService.GetAsync(cancellationToken);

        return Ok(settings);
    }

    /// <summary>
    /// Replaces the baker's numbers. Every product reprices on the next read.
    /// </summary>
    /// <param name="request">Income, routine, fixed costs and the pricing defaults.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>The settings as they were stored, with the new hourly cost.</returns>
    /// <response code="200">The settings were replaced.</response>
    /// <response code="400">A value is out of range, or the fees take the whole price.</response>
    [HttpPut]
    [ProducesResponseType(typeof(BakerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BakerResponse>> Update(
        [FromBody] BakerRequest request,
        CancellationToken cancellationToken)
    {
        var settings = await _bakerService.UpdateAsync(request, cancellationToken);

        return Ok(settings);
    }

    /// <summary>
    /// Works out an hourly cost from numbers that are not stored, so a form can
    /// show the result while it is being filled in.
    /// </summary>
    /// <param name="request">The numbers being tried out.</param>
    /// <returns>The hourly cost those numbers would produce.</returns>
    /// <response code="200">The simulated hourly cost.</response>
    /// <response code="400">A value is out of range, or the fees take the whole price.</response>
    [HttpPost("preview")]
    [ProducesResponseType(typeof(HourlyCostResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<HourlyCostResponse> Preview([FromBody] BakerRequest request)
    {
        return Ok(_bakerService.Preview(request));
    }
}
