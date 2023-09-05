using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.UseCases;

namespace MovieApp.Core.Tests.Users.UseCases;

public class GetAllMoviesUseCaseTest
{
    private Mock<IUserRepository> _userRepositoryMock = null!;
    private GetAllUsersUseCase _getAllUsersUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _getAllUsersUseCase = new GetAllUsersUseCase(_userRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WithValidId_ShouldBeSuccess()
    {
        // Arrange
        var users = _fixture.CreateMany<User>().ToList();
        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        IEnumerable<UserDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getAllUsersUseCase.ExecuteAsync();
        
        // Assert
        await act.Should().NotThrowAsync();
        var userDtos = result.ToList();
        userDtos.Should().NotBeNullOrEmpty();
        userDtos.Should().HaveCount(users.Count);
        _userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }
    
    [Test]
    public async Task ExecuteAsync_WithNoUsers_ShouldReturnEmptyList()
    {
        // Arrange
        List<User> users = null!;
        users = new List<User>();
        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        IEnumerable<UserDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getAllUsersUseCase.ExecuteAsync();
        
        // Assert
        await act.Should().NotThrowAsync();
        var userDtos = result.ToList();
        userDtos.Should().BeEmpty();
        _userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }
}