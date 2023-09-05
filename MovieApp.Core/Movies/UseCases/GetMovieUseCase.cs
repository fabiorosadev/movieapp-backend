using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Ports.Persistence;
using Serilog;

namespace MovieApp.Core.Movies.UseCases;

public class GetMovieUseCase : IGetByIdUseCase<MovieDto, Guid>
{
    private readonly IMovieRepository _movieRepository;
    
    public GetMovieUseCase(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<MovieDto> ExecuteAsync(Guid id)
    {
        Log.Logger.Information("Execute {UseCase} for Id {Id}", nameof(GetMovieUseCase), id);
        var movie = await _movieRepository.GetAsync(id);
        if (movie is null)
        {
            Log.Logger.Information("No movie found for Id {Id}", id);
            throw new NotFoundException("Movie not found");
        }
        return MovieDto.FromMovie(movie);
    }
}