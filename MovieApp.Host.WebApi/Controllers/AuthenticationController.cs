using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Users.UseCases.Interfaces;
using MovieApp.Host.WebApi.Authorization.Interfaces;
using MovieApp.Host.WebApi.Models.Authentication;

namespace MovieApp.Host.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticateUserUseCase _authenticateUserUseCase;
    private readonly IJwtUtils _jwtUtils;
    
    public AuthenticationController(IAuthenticateUserUseCase authenticateUserUseCase, IJwtUtils jwtUtils)
    {
        _authenticateUserUseCase = authenticateUserUseCase;
        _jwtUtils = jwtUtils;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Authenticate(AuthenticationRequestModel model)
    {
        var user = await _authenticateUserUseCase.ExecuteAsync(model.ToAuthUserDto());
        
        if (user is null)
        {
            return BadRequest(new { message = "Username or password is incorrect" });
        }
        
        var authUser = new AuthUser(user.Id);

        var token = _jwtUtils.GenerateJwtToken(authUser);
        var response = new AuthenticationResponseModel(token);

        return Ok(response);
    }
}