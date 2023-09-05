using FluentValidation;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Users.Validations;

namespace MovieApp.Core.Users.Dtos;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

    private IValidator<UpdateUserDto> Validator { get; } = new UpdateUserDtoValidation();
    
    public void Validate()
    {
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            throw new InputValidationException(string.Join(Environment.NewLine, validationResult.Errors));
        }
    }
}