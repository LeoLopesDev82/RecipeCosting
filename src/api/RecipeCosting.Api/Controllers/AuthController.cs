using Microsoft.AspNetCore.Mvc;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Services.Auth;

namespace RecipeCosting.Api.Controllers;

/// <summary>
/// Signing in. Every other endpoint expects the token this one returns.
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Exchanges the demonstration credentials for a bearer token.
    /// </summary>
    /// <param name="request">Email and password of the demonstration account.</param>
    /// <returns>The token and the moment it expires.</returns>
    /// <response code="200">The credentials matched.</response>
    /// <response code="400">The request is missing the email or the password.</response>
    /// <response code="401">The credentials did not match.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var session = _authService.SignIn(request);

        if (session == null)
            return Unauthorized();

        return Ok(session);
    }
}
