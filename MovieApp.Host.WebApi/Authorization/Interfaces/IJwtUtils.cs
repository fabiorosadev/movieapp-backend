using MovieApp.Host.WebApi.Models.Authentication;

namespace MovieApp.Host.WebApi.Authorization.Interfaces;

public interface IJwtUtils
{
    public string GenerateJwtToken(AuthUser user);
    public Guid? ValidateJwtToken(string? token);
}