using FluentValidation;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.Validations;

namespace MovieApp.Core.Movies.Dtos;

public class UpdateMovieDto : Movie
{
    private IValidator<UpdateMovieDto> Validator { get; } = new UpdateMovieDtoValidation();
    
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
            Id = Id,
            Title = Title,
            Description = Description,
            Genre = Genre,
            CoverUrl = CoverUrl,
            ReleaseDate = ReleaseDate,
            ImDbRating = ImDbRating,
            ImDbUrl = ImDbUrl
        };
    }
}