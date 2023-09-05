using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.UseCases.Interfaces;
using Serilog;

namespace MovieApp.Core.Users.UseCases;

public class GetUserByEmailUseCase : IGetUserByEmailUseCase
{
    private readonly IUserRepository _userRepository;
    
    public GetUserByEmailUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDto?> ExecuteAsync(string email)
    {
        Log.Logger.Information("Execute {UseCase} for Email {Email}", nameof(GetUserByEmailUseCase), email);
        var user =  await _userRepository.GetByEmailAsync(email);
        return user is null ? null : UserDto.FromUser(user);
    }
}