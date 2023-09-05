using MovieApp.Core.Movies.Entities;

namespace MovieApp.Core.Ports.Persistence;

public interface IMovieRepository
{
    Task<Movie> GetAsync(Guid id);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie> CreateAsync(Movie movie);
    Task<Movie> UpdateAsync(Movie movie);
    Task DeleteAsync(Guid id);
}