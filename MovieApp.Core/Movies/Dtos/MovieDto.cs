using MovieApp.Core.Movies.Entities;

namespace MovieApp.Core.Movies.Dtos;

public class MovieDto : Movie
{
    public static MovieDto FromMovie(Movie movie)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Genre = movie.Genre,
            CoverUrl = movie.CoverUrl,
            ReleaseDate = movie.ReleaseDate,
            ImDbRating = movie.ImDbRating,
            ImDbUrl = movie.ImDbUrl
        };
    }
}