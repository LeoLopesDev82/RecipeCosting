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
}
