using MovieApp.Core.Users.Dtos;

namespace MovieApp.Host.WebApi.Models.Authentication;

public class AuthenticationRequestModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public AuthUserDto ToAuthUserDto()
    {
        return new AuthUserDto(Email, Password);
    }
}