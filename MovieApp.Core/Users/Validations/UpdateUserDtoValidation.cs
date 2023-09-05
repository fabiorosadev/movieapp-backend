using FluentValidation;
using MovieApp.Core.Users.Dtos;

namespace MovieApp.Core.Users.Validations;

public class UpdateUserDtoValidation : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidation()
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
            .MinimumLength(8)
            .When(u => u.Password != null);

        RuleFor(u => u.ConfirmPassword)
            .NotEmpty()
            .Equal(u => u.Password)
            .When(u => u.Password != null);
    }
}