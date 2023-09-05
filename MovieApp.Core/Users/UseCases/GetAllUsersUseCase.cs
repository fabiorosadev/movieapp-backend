using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using Serilog;

namespace MovieApp.Core.Users.UseCases;

public class GetAllUsersUseCase : IGetAllUseCase<UserDto>
{
    private readonly IUserRepository _userRepository;
    
    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<UserDto>> ExecuteAsync()
    {
        Log.Logger.Information("Execute {UseCase}", nameof(GetAllUsersUseCase));
        var users = await _userRepository.GetAllAsync();
        return users.Select(UserDto.FromUser);
    }
}