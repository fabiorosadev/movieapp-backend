using System.Diagnostics.CodeAnalysis;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Users.Dtos;
using MovieApp.Host.WebApi.Authorization.Interfaces;

namespace MovieApp.Host.WebApi.Authorization;

[ExcludeFromCodeCoverage]
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    
    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context, IGetByIdUseCase<UserDto, Guid> getUserById, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = await getUserById.ExecuteAsync(userId.Value);
        }

        await _next(context);
    }
}