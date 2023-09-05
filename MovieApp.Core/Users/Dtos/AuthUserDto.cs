using FluentValidation;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Users.Validations;

namespace MovieApp.Core.Users.Dtos;

public class AuthUserDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    private IValidator<AuthUserDto> Validator { get; } = new AuthUserDtoValidation();
    
    public void Validate()
    {
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            throw new InputValidationException(string.Join(Environment.NewLine, validationResult.Errors));
        }
    }
    public AuthUserDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
}