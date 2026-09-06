namespace RecipeCosting.Api.Models.Responses;

/// <summary>
/// The token to send on every later request, and when it stops working.
/// </summary>
public class LoginResponse
{
    /// <summary>Email the token was issued to.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Bearer token, sent as "Authorization: Bearer {token}".</summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>Moment the token expires, in UTC.</summary>
    public DateTime ExpiresAt { get; set; }
}
