using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Ports.Persistence;
using Serilog;

namespace MovieApp.Core.Movies.UseCases;

public class CreateMovieUseCase : ICreateUseCase<MovieDto, CreateMovieDto>
{
    private readonly IMovieRepository _movieRepository;
    
    public CreateMovieUseCase(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<MovieDto> ExecuteAsync(CreateMovieDto createMovieDto)
    {
        createMovieDto.Validate();
        Log.Logger.Information("Execute {UseCase} for {@Movie}", nameof(CreateMovieUseCase), createMovieDto);
        var createdMovie = await _movieRepository.CreateAsync(createMovieDto.ToMovie());
        return MovieDto.FromMovie(createdMovie);
    }
}