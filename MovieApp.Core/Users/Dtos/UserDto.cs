using MovieApp.Core.Users.Entities;

namespace MovieApp.Core.Users.Dtos;

public class UserDto : User
{
    public static UserDto FromUser(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role
        };
    }
}