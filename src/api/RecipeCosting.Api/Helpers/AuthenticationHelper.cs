using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RecipeCosting.Api.Models.Settings;

namespace RecipeCosting.Api.Helpers;

/// <summary>
/// Wires bearer authentication to the signing key held in configuration.
/// </summary>
public static class AuthenticationHelper
{
    /// <summary>
    /// Registers the auth settings and the token validation rules.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    /// <param name="configuration">Configuration carrying the Auth section.</param>
    public static void AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(AuthSettings.SectionName);
        var settings = section.Get<AuthSettings>() ?? new AuthSettings();

        Demand(settings.SigningKey);

        services.Configure<AuthSettings>(section);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey)),
                    ClockSkew = TimeSpan.Zero,
                };
            });
    }

    private static void Demand(string signingKey)
    {
        if (signingKey.Length >= 32)
            return;

        throw new InvalidOperationException(
            "Auth:SigningKey is missing or shorter than 32 characters. Copy "
            + "appsettings.Development.example.json over appsettings.Development.json and put a "
            + "key in it, or set Auth__SigningKey in the environment. The repository never "
            + "carries one.");
    }
}
