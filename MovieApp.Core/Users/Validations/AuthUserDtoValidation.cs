using FluentValidation;
using MovieApp.Core.Users.Dtos;

namespace MovieApp.Core.Users.Validations;

public class AuthUserDtoValidation : AbstractValidator<AuthUserDto>
{
    public AuthUserDtoValidation()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}