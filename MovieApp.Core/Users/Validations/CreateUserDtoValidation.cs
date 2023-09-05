using FluentValidation;
using MovieApp.Core.Users.Dtos;

namespace MovieApp.Core.Users.Validations;

public class CreateUserDtoValidation : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidation()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(u => u.LastName)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);
        
        RuleFor(u => u.ConfirmPassword)
            .NotEmpty()
            .Equal(u => u.Password);
    }
}