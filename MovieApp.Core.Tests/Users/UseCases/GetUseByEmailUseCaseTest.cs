using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.UseCases;

namespace MovieApp.Core.Tests.Users.UseCases;

public class GetUseByEmailUseCaseTest
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private GetUserByEmailUseCase _getUserByEmailUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _getUserByEmailUseCase = new GetUserByEmailUseCase(_userRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WithValidEmail_ShouldBeSuccess()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(user);
        User? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getUserByEmailUseCase.ExecuteAsync(email);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user, options => options.Excluding(u => u.PasswordHash));
        _userRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
    }
    
    [Test]
    public async Task ExecuteAsync_WithInvalidEmail_ShouldReturnNull()
    {
        // Arrange
        var email = string.Empty;
        User? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getUserByEmailUseCase.ExecuteAsync(email);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().BeNull();
    }
}