using FluentValidation;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Enums;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.Validations;

namespace MovieApp.Core.Users.Dtos;

public class CreateUserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    
    public User ToUser(Role role = Role.User)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password),
            Role = role
        };
    }

    private IValidator<CreateUserDto> Validator { get; } = new CreateUserDtoValidation();
    
    public void Validate()
    {
        var validationResult = Validator.Validate(this);
        if (!validationResult.IsValid)
        {
            throw new InputValidationException(string.Join(Environment.NewLine, validationResult.Errors));
        }
    }
}