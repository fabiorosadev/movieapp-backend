using Microsoft.AspNetCore.Mvc;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Movies.Entities;
using MovieApp.Host.WebApi.Authorization;

namespace MovieApp.Host.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IGetAllUseCase<MovieDto> _getAllUseCase;
    private readonly IGetByIdUseCase<MovieDto, Guid> _getByIdUseCase;
    private readonly ICreateUseCase<MovieDto, CreateMovieDto> _createUseCase;
    private readonly IUpdateUseCase<MovieDto, UpdateMovieDto> _updateUseCase;
    private readonly IDeleteUseCase<Movie, Guid> _deleteUseCase;
    
    public MoviesController(
        IGetAllUseCase<MovieDto> getAllUseCase,
        IGetByIdUseCase<MovieDto, Guid> getByIdUseCase,
        ICreateUseCase<MovieDto, CreateMovieDto> createUseCase,
        IUpdateUseCase<MovieDto, UpdateMovieDto> updateUseCase,
        IDeleteUseCase<Movie, Guid> deleteUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _getByIdUseCase = getByIdUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<MovieDto>> GetAll()
    {
        return await _getAllUseCase.ExecuteAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<MovieDto?> GetById(Guid id)
    {
        return await _getByIdUseCase.ExecuteAsync(id);
    }
    
    [HttpPost]
    public async Task<MovieDto> Create(CreateMovieDto createMovie)
    {
        return await _createUseCase.ExecuteAsync(createMovie);
    }
    
    [HttpPut]
    public async Task<MovieDto> Update(UpdateMovieDto updateMovie)
    {
        return await _updateUseCase.ExecuteAsync(updateMovie);
    }
    
    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        await _deleteUseCase.ExecuteAsync(id);
    }
}