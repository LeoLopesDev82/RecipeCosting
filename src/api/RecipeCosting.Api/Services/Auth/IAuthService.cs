using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;

namespace RecipeCosting.Api.Services.Auth;

/// <summary>
/// Turns credentials into a signed token.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Checks the credentials and issues a token, or returns null when they do not match.
    /// </summary>
    LoginResponse? SignIn(LoginRequest request);
}
