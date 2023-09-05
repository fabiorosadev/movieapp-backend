using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Host.WebApi.Authorization;

namespace MovieApp.Host.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IGetAllUseCase<UserDto> _getAllUseCase;
    private readonly IGetByIdUseCase<UserDto, Guid> _getByIdUseCase;
    private readonly ICreateUseCase<UserDto, CreateUserDto> _createUseCase;
    private readonly IUpdateUseCase<UserDto, UpdateUserDto> _updateUseCase;
    private readonly IDeleteUseCase<User, Guid> _deleteUseCase;
    
    public UsersController(
        IGetAllUseCase<UserDto> getAllUseCase,
        IGetByIdUseCase<UserDto, Guid> getByIdUseCase,
        ICreateUseCase<UserDto, CreateUserDto> createUseCase,
        IUpdateUseCase<UserDto, UpdateUserDto> updateUseCase,
        IDeleteUseCase<User, Guid> deleteUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _getByIdUseCase = getByIdUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }
    
    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetAll()
    {
        return await _getAllUseCase.ExecuteAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<UserDto?> GetById(Guid id)
    {
        return await _getByIdUseCase.ExecuteAsync(id);
    }
    
    [HttpPost]
    public async Task<UserDto> Create(CreateUserDto createUser)
    {
        return await _createUseCase.ExecuteAsync(createUser);
    }
    
    [HttpPut]
    public async Task<UserDto> Update(UpdateUserDto updateUser)
    {
        return await _updateUseCase.ExecuteAsync(updateUser);
    }
    
    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        await _deleteUseCase.ExecuteAsync(id);
    }
}