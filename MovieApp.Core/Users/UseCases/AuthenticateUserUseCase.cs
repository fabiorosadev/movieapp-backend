using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.UseCases.Interfaces;

namespace MovieApp.Core.Users.UseCases;

public class AuthenticateUserUseCase : IAuthenticateUserUseCase
{
    private readonly IUserRepository _userRepository;
    
    public AuthenticateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDto?> ExecuteAsync(AuthUserDto authUserDto)
    {
        authUserDto.Validate();
        var user = await _userRepository.GetByEmailAsync(authUserDto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(authUserDto.Password, user.PasswordHash))
        {
            return null;
        }
        
        return UserDto.FromUser(user);
    }
}