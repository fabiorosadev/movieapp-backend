using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Ports.Persistence;
using Serilog;

namespace MovieApp.Core.Movies.UseCases;

public class DeleteMovieUseCase : IDeleteUseCase<Movie, Guid>
{
    private readonly IMovieRepository _movieRepository;
    
    public DeleteMovieUseCase(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task ExecuteAsync(Guid entityId)
    {
        if(entityId == Guid.Empty)
        {
            Log.Logger.Error("Invalid Id {Id}", entityId);
            throw new InputValidationException("Invalid Id");
        }
        var movie = await _movieRepository.GetAsync(entityId);
        if(movie is null)
        {
            Log.Logger.Error("No movie found for Id {Id}", entityId);
            throw new NotFoundException("Movie not found");
        }
        Log.Logger.Information("Execute {UseCase} for Id {Id}", nameof(DeleteMovieUseCase), entityId);    
        await _movieRepository.DeleteAsync(entityId);
    }
}