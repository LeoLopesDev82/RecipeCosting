using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeCosting.Api.Models.Requests;
using RecipeCosting.Api.Models.Responses;
using RecipeCosting.Api.Models.Settings;

namespace RecipeCosting.Api.Services.Auth;

/// <inheritdoc cref="IAuthService"/>
public class AuthService : IAuthService
{
    private readonly AuthSettings _settings;

    public AuthService(IOptions<AuthSettings> settings)
    {
        _settings = settings.Value;
    }

    public LoginResponse? SignIn(LoginRequest request)
    {
        if (!Matches(request))
            return null;

        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.LifetimeMinutes);

        return new LoginResponse
        {
            Email = _settings.DemoEmail,
            AccessToken = WriteToken(_settings.DemoEmail, expiresAt),
            ExpiresAt = expiresAt,
        };
    }

    #region Private methods

    private bool Matches(LoginRequest request)
    {
        return string.Equals(request.Email.Trim(), _settings.DemoEmail, StringComparison.OrdinalIgnoreCase)
            && request.Password == _settings.DemoPassword;
    }

    private string WriteToken(string email, DateTime expiresAt)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey));

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: [new Claim(JwtRegisteredClaimNames.Sub, email), new Claim(JwtRegisteredClaimNames.Email, email)],
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    #endregion
}
