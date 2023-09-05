using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.UseCases;

namespace MovieApp.Core.Tests.Users.UseCases;

public class AuthenticateUserUseCaseTest
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private AuthenticateUserUseCase _authenticateUserUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _userRepositoryMock = new Mock<IUserRepository>();
        _authenticateUserUseCase = new AuthenticateUserUseCase(_userRepositoryMock.Object);
    }
    
    [Test]
    public async Task ExecuteAsync_WithValidCredentials_ShouldBeSuccess()
    {
        // Arrange
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("password");
        var user = _fixture.Build<User>()
            .With(x => x.PasswordHash, passwordHash)
            .With(x => x.Email, "test@test.com")
            .Create();
        var authenticateUserDto = _fixture.Build<AuthUserDto>()
            .With(x => x.Password, "password")
            .With(x => x.Email, "test@test.com")
            .Create();
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(authenticateUserDto.Email)).ReturnsAsync(user);
        UserDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _authenticateUserUseCase.ExecuteAsync(authenticateUserDto);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Email.Should().Be(user.Email);
    }

    [Test]
    public async Task ExecuteAsync_WithInvalidCredentials_ShouldThrowValidationException()
    {
        // Arrange
        var authUser = _fixture.Build<AuthUserDto>()
            .With(x => x.Email, "noValidEmail")
            .Create();

        // Act
        Func<Task> act = async () => await _authenticateUserUseCase.ExecuteAsync(authUser);

        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
    }
    
    [Test]
    public async Task ExecuteAsync_WithUserNotFound_ShouldReturnNull()
    {
        // Arrange
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("password");
        User user = null!;
        var authenticateUserDto = _fixture.Build<AuthUserDto>()
            .With(x => x.Password, "password")
            .With(x => x.Email, "test@test.com")
            .Create();
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(authenticateUserDto.Email)).ReturnsAsync(user);
        UserDto? result = null!;

        // Act
        Func<Task> act = async () => result = await _authenticateUserUseCase.ExecuteAsync(authenticateUserDto);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().BeNull();
    }
}