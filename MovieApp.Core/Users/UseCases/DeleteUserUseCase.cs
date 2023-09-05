using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Entities;
using Serilog;

namespace MovieApp.Core.Users.UseCases;

public class DeleteUserUseCase : IDeleteUseCase<User, Guid>
{
    private readonly IUserRepository _userRepository;
    
    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task ExecuteAsync(Guid entityId)
    {
        if(entityId == Guid.Empty)
        {
            Log.Logger.Error("Invalid Id {Id}", entityId);
            throw new InputValidationException("Invalid Id");
        }
        Log.Logger.Information("Execute {UseCase} for {@Id}", nameof(DeleteUserUseCase), entityId);
        var entity = await _userRepository.GetAsync(entityId);
        if (entity is null)
        {
            Log.Logger.Warning("Entity with id {@Id} not found", entityId);
            throw new NotFoundException("Entity not found");
        }
        await _userRepository.DeleteAsync(entityId);
    }
}