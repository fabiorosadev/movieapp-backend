namespace MovieApp.Host.WebApi.Models.Authentication;

public class AuthenticationResponseModel
{
    public string Token { get; set; }

    public AuthenticationResponseModel(string token)
    {
        Token = token;
    }
}