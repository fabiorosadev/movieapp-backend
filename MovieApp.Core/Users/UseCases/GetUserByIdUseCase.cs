using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using Serilog;

namespace MovieApp.Core.Users.UseCases;

public class GetUserByIdUseCase : IGetByIdUseCase<UserDto, Guid>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserByIdUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDto> ExecuteAsync(Guid id)
    {
        Log.Logger.Information("Execute {UseCase} for Id {Id}", nameof(GetUserByIdUseCase), id);
        var user = await _userRepository.GetAsync(id);
        if (user is null)
        {
            Log.Logger.Warning("User with Id {Id} was not found", id);
            throw new NotFoundException($"User with Id {id} was not found");
        }
        return UserDto.FromUser(user);
    }
}