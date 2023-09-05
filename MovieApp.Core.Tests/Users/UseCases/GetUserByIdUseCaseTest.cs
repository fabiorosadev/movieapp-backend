using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.UseCases;

namespace MovieApp.Core.Tests.Users.UseCases;

public class GetUserByIdUseCaseTest
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private GetUserByIdUseCase _getUserByIdUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _getUserByIdUseCase = new GetUserByIdUseCase(_userRepositoryMock.Object);
        _fixture = new Fixture();
    }

    [Test]
    public async Task ExecuteAsync_WhenUserExists_ShouldBeSuccess()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var user = _fixture.Create<User>();
        _userRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(user);
        UserDto result = null!;

        // Act
        Func<Task> act = async () => result = await _getUserByIdUseCase.ExecuteAsync(id);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user, options => options.Excluding(u => u.PasswordHash));
    }
    
    [Test]
    public async Task ExecuteAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        User user = null!;
        _userRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _getUserByIdUseCase.ExecuteAsync(id);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}