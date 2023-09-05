using MovieApp.Core.Users.Dtos;

namespace MovieApp.Core.Users.UseCases.Interfaces;

public interface IAuthenticateUserUseCase
{
    Task<UserDto?> ExecuteAsync(AuthUserDto authUserDto);
}