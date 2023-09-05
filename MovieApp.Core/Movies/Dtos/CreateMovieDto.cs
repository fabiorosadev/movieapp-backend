using FluentValidation;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Enums;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.Validations;

namespace MovieApp.Core.Movies.Dtos;

public class CreateMovieDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public Genre Genre { get; set; }
    public string CoverUrl { get; set; } = null!;
    public string ImDbUrl { get; set; } = null!;
    public decimal ImDbRating { get; set; }
    
    private IValidator<CreateMovieDto> Validator { get; } = new CreateMovieDtoValidation();

    public void Validate()
    {
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            throw new InputValidationException(string.Join(Environment.NewLine, validationResult.Errors));
        }
    }
    
    public Movie ToMovie()
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            Title = Title,
            Description = Description,
            ReleaseDate = ReleaseDate,
            Genre = Genre,
            CoverUrl = CoverUrl,
            ImDbUrl = ImDbUrl,
            ImDbRating = ImDbRating
        };
    }
}