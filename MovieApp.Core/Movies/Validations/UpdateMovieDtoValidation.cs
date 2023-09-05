using FluentValidation;
using MovieApp.Core.Movies.Dtos;

namespace MovieApp.Core.Movies.Validations;

public class UpdateMovieDtoValidation : AbstractValidator<UpdateMovieDto>
{
    public UpdateMovieDtoValidation()
    {
        RuleFor(m => m.Id)
            .NotEmpty();
        
        RuleFor(m => m.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(m => m.Description)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(m => m.ReleaseDate)
            .NotEmpty();

        RuleFor(m => m.Genre)
            .IsInEnum();

        RuleFor(m => m.CoverUrl)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(m => m.ImDbUrl)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(m => m.ImDbRating)
            .NotEmpty();
    }
}