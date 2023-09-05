using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.Exceptions;
using MovieApp.Core.Users.UseCases;

namespace MovieApp.Core.Tests.Users.UseCases;

public class UpdateUserUseCaseTest
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private UpdateUserUseCase _updateUserUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _updateUserUseCase = new UpdateUserUseCase(_userRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WhenValidUpdateUser_WithoutPasswordChange_ShouldBeSuccess()
    {
        // Arrange
        var updateUserDto = _fixture.Build<UpdateUserDto>()
            .With(u => u.Email, "test@test.com")
            .Without(u => u.Password)
            .Without(u => u.ConfirmPassword)
            .Create();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(x => x.GetAsync(updateUserDto.Id)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);
        UserDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _updateUserUseCase.ExecuteAsync(updateUserDto);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        
    }

    [Test]
    public async Task ExecuteAsync_WhenValidUpdateUser_WithPasswordChange_ShouldBeSuccess()
    {
        // Arrange
        var updateUserDto = _fixture.Build<UpdateUserDto>()
            .With(u => u.Email, "test@test.com")
            .With(u => u.Password, "test1234")
            .With(u => u.ConfirmPassword, "test1234")
            .Create();
        
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(x => x.GetAsync(updateUserDto.Id)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);
        UserDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _updateUserUseCase.ExecuteAsync(updateUserDto);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        
    }
    
    [Test]
    [TestCase("invalidEmail", null, null, TestName = "WhenInvalidEmail")]
    [TestCase("test@test.com", "test1234", "test12345", TestName = "WhenInvalidPassword")]
    public async Task ExecuteAsync_WhenInvalidUpdateUser_ShouldThrowInvalidInputException(string email, string? password, string? confirmPassword)
    {
        // Arrange
        var updateUserDto = _fixture.Build<UpdateUserDto>()
            .With(u => u.Email, email)
            .With(u => u.Password, password)
            .With(u => u.ConfirmPassword, confirmPassword)
            .Create();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(x => x.GetAsync(updateUserDto.Id)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);
        UserDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _updateUserUseCase.ExecuteAsync(updateUserDto);
        
        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
        result.Should().BeNull();
    }
    
    
    [Test]
    public async Task ExecuteAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var updateUserDto = _fixture.Build<UpdateUserDto>()
            .With(u => u.Email, "test@test.com")
            .Without(u => u.Password)
            .Without(u => u.ConfirmPassword)
            .Create();
        User user = null!;
        _userRepositoryMock.Setup(x => x.GetAsync(updateUserDto.Id)).ReturnsAsync(user);
        
        // Act
        Func<Task> act = async () => await _updateUserUseCase.ExecuteAsync(updateUserDto);
        
        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
}