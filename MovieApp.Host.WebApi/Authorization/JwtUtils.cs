using System.Diagnostics.CodeAnalysis;
using MovieApp.Host.WebApi.Authorization.Interfaces;
using MovieApp.Host.WebApi.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MovieApp.Host.WebApi.Authorization;

[ExcludeFromCodeCoverage]
public class JwtUtils : IJwtUtils
{
    private readonly JwtSettings _jwtSettings;
    
    public JwtUtils(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
        if (string.IsNullOrEmpty(_jwtSettings.Secret))
            throw new Exception("JWT secret not configured");
    }
    
    public string GenerateJwtToken(AuthUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Guid? ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }
}