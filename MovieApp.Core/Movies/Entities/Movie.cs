using MovieApp.Core.Enums;

namespace MovieApp.Core.Movies.Entities;

public class Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public Genre Genre { get; set; }
    public string CoverUrl { get; set; } = null!;
    public string ImDbUrl { get; set; } = null!;
    public decimal ImDbRating { get; set; }
}