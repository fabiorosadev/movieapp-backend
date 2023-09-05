namespace MovieApp.Host.WebApi.Models.Authentication;

public class AuthUser
{
    public Guid Id { get; set; }

    public AuthUser(Guid id)
    {
        Id = id;
    }
}