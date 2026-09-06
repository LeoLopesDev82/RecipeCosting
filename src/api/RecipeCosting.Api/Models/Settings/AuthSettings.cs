namespace RecipeCosting.Api.Models.Settings;

/// <summary>
/// How tokens are signed, and the single account this demonstration accepts.
/// </summary>
public class AuthSettings
{
    /// <summary>Configuration section these settings are read from.</summary>
    public const string SectionName = "Auth";

    /// <summary>Who issues the token.</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Who the token is meant for.</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Secret the token is signed with, at least 32 characters. It is never kept
    /// in the repository: user secrets hold it while developing, and an
    /// environment variable holds it anywhere else.
    /// </summary>
    public string SigningKey { get; set; } = string.Empty;

    /// <summary>How long a token stays valid, in minutes.</summary>
    public int LifetimeMinutes { get; set; }

    /// <summary>Email of the demonstration account.</summary>
    public string DemoEmail { get; set; } = string.Empty;

    /// <summary>Password of the demonstration account.</summary>
    public string DemoPassword { get; set; } = string.Empty;
}
