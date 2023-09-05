using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Ports.Persistence;
using Serilog;

namespace MovieApp.Core.Movies.UseCases;

public class UpdateMovieUseCase : IUpdateUseCase<MovieDto, UpdateMovieDto>
{
    private readonly IMovieRepository _movieRepository;
    
    public UpdateMovieUseCase(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<MovieDto> ExecuteAsync(UpdateMovieDto updateMovieDto)
    {
        updateMovieDto.Validate();
        Log.Logger.Information("Execute {UseCase} for {@Movie}", nameof(UpdateMovieUseCase), updateMovieDto);
        var movie = await _movieRepository.GetAsync(updateMovieDto.Id);
        if (movie is null)
        {
            Log.Logger.Information("No movie found for Id {Id}", updateMovieDto.Id);
            throw new NotFoundException("Movie not found");
        }
        var movieUpdated = await _movieRepository.UpdateAsync(updateMovieDto.ToMovie());
        return MovieDto.FromMovie(movieUpdated);
    }
}