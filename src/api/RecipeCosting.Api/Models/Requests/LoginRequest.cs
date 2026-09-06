using System.ComponentModel.DataAnnotations;

namespace RecipeCosting.Api.Models.Requests;

/// <summary>
/// The credentials sent to obtain a token.
/// </summary>
public class LoginRequest
{
    /// <summary>Email of the account signing in.</summary>
    [Required(ErrorMessage = "The email is required.")]
    [EmailAddress(ErrorMessage = "The email is not a valid address.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Password of the account signing in.</summary>
    [Required(ErrorMessage = "The password is required.")]
    public string Password { get; set; } = string.Empty;
}
