using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Ports.Persistence;
using Serilog;

namespace MovieApp.Core.Movies.UseCases;

public class GetAllMoviesUseCase : IGetAllUseCase<MovieDto>
{
    private readonly IMovieRepository _movieRepository;
    
    public GetAllMoviesUseCase(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<IEnumerable<MovieDto>> ExecuteAsync()
    {
        Log.Logger.Information("Execute {UseCase}", nameof(GetAllMoviesUseCase));
        var movies = await _movieRepository.GetAllAsync();
        return movies.Select(MovieDto.FromMovie);
    }
}