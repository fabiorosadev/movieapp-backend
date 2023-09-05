using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Exceptions;
using Serilog;

namespace MovieApp.Core.Users.UseCases;

public class UpdateUserUseCase : IUpdateUseCase<UserDto, UpdateUserDto>
{
    private readonly IUserRepository _userRepository;
    
    public UpdateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDto> ExecuteAsync(UpdateUserDto updateMovieDto)
    {
        updateMovieDto.Validate();
        var user = await _userRepository.GetAsync(updateMovieDto.Id);
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }
        user!.FirstName = updateMovieDto.FirstName;
        user.LastName = updateMovieDto.LastName;
        user.Email = updateMovieDto.Email;
        if (updateMovieDto.Password is not null)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateMovieDto.Password);
        }
        
        Log.Logger.Information("Execute {UseCase} for {@User}", nameof(UpdateUserUseCase), user);
        var updatedUser = await _userRepository.UpdateAsync(user);
        return UserDto.FromUser(updatedUser);
    }
}