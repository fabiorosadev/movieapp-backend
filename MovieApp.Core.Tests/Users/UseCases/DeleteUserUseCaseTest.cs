using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.UseCases;

namespace MovieApp.Core.Tests.Users.UseCases;

public class DeleteUserUseCaseTest
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private DeleteUserUseCase _deleteUserUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _deleteUserUseCase = new DeleteUserUseCase(_userRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WithValidId_ShouldBeSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(x => x.GetAsync(userId)).ReturnsAsync(user);
        
        // Act
        Func<Task> act = async () => await _deleteUserUseCase.ExecuteAsync(userId);
        
        // Assert
        await act.Should().NotThrowAsync();
        _userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Once);
    }
    
    [Test]
    public async Task ExecuteAsync_WithInvalidId_ShouldThrowValidationException()
    {
        // Arrange
        var userId = Guid.Empty;
        
        // Act
        Func<Task> act = async () => await _deleteUserUseCase.ExecuteAsync(userId);
        
        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
    }
    
    [Test]
    public async Task ExecuteAsync_WithNonExistingId_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        User user = null!;
        _userRepositoryMock.Setup(x => x.GetAsync(userId)).ReturnsAsync(user);
        
        // Act
        Func<Task> act = async () => await _deleteUserUseCase.ExecuteAsync(userId);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    
}