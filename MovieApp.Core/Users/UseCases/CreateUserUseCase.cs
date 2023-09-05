using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using Serilog;

namespace MovieApp.Core.Users.UseCases;

public class CreateUserUseCase : ICreateUseCase<UserDto, CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDto> ExecuteAsync(CreateUserDto createMovieDto)
    {
        createMovieDto.Validate();
        var user = createMovieDto.ToUser();
        Log.Logger.Information("Execute {UseCase} for {@User}", nameof(CreateUserUseCase), user);
        var existingUser = await _userRepository.GetByEmailAsync(user.Email);
        if(existingUser is not null)
        {
            Log.Logger.Error("User with email {Email} already exists", user.Email);
            throw new InputValidationException("User with email already exists");
        }
        var createdUser = await _userRepository.CreateAsync(user); 
        return UserDto.FromUser(createdUser);
    }
}