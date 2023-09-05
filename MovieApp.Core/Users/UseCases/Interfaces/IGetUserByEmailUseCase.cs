using MovieApp.Core.Users.Dtos;

namespace MovieApp.Core.Users.UseCases.Interfaces;

public interface IGetUserByEmailUseCase
{
    Task<UserDto?> ExecuteAsync(string email);
}