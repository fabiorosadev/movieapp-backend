using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.UseCases;

namespace MovieApp.Core.Tests.Users.UseCases;

public class CreateUserUseCaseTest
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private CreateUserUseCase _createUserUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _userRepositoryMock = new Mock<IUserRepository>();
        _createUserUseCase = new CreateUserUseCase(_userRepositoryMock.Object);
    }
    
    [Test]
    public async Task ExecuteAsync_WithValidCreateUserDto_ShouldBeSuccess()
    {
        // Arrange
        var createUserDto = _fixture.Build<CreateUserDto>()
            .With(u => u.Email, "test@test.com")
            .With(u => u.Password, "password")
            .With(u => u.ConfirmPassword, "password")
            .Create();
        UserDto result = null!;
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createUserDto.ToUser());
        
        // Act
        Func<Task> act = async () => result = await _createUserUseCase.ExecuteAsync(createUserDto);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createUserDto, options => options.ExcludingMissingMembers());
    }

    [Test]
    public async Task ExecuteAsync_WithInvalidCreateUserDto_ShouldThrowValidationException()
    {
        // Arrange
        var createUserDto = _fixture.Build<CreateUserDto>()
            .With(u => u.Email, "invalid")
            .Create();
        UserDto result = null!;

        // Act
        Func<Task> act = async () => result = await _createUserUseCase.ExecuteAsync(createUserDto);

        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
        result.Should().BeNull();
    }

    [Test]
    public async Task ExecuteAsync_WithNonMatchingPasswords_ShouldThrowValidationException()
    {
        // Arrange
        var createUserDto = _fixture.Build<CreateUserDto>()
            .With(u => u.Email, "test@test.com")
            .With(u => u.Password, "password")
            .With(u => u.ConfirmPassword, "notMatching")
            .Create();
        UserDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _createUserUseCase.ExecuteAsync(createUserDto);
        
        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
        result.Should().BeNull();
    }

    [Test]
    public async Task ExecuteAsync_WithExistingEmail_ShouldThrowValidationException()
    {
        // Arrange
        var createUserDto = _fixture.Build<CreateUserDto>()
            .With(u => u.Email, "test@test.com")
            .With(u => u.Password, "password")
            .With(u => u.ConfirmPassword, "password")
            .Create();
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(createUserDto.Email)).ReturnsAsync(createUserDto.ToUser());
        UserDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _createUserUseCase.ExecuteAsync(createUserDto);
        
        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
        result.Should().BeNull();
    }
}