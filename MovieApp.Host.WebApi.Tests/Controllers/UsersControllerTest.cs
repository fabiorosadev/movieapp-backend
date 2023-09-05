using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Host.WebApi.Controllers;

namespace MovieApp.Host.WebApi.Tests.Controllers;

public class UsersControllerTest
{
    private Mock<IGetAllUseCase<UserDto>> _getAllUseCaseMock = null!;
    private Mock<IGetByIdUseCase<UserDto, Guid>> _getByIdUseCaseMock = null!;
    private Mock<ICreateUseCase<UserDto, CreateUserDto>> _createUseCaseMock = null!;
    private Mock<IUpdateUseCase<UserDto, UpdateUserDto>> _updateUseCaseMock = null!;
    private Mock<IDeleteUseCase<User, Guid>> _deleteUseCaseMock = null!;
    private UsersController _usersController = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _getAllUseCaseMock = new Mock<IGetAllUseCase<UserDto>>();
        _getByIdUseCaseMock = new Mock<IGetByIdUseCase<UserDto, Guid>>();
        _createUseCaseMock = new Mock<ICreateUseCase<UserDto, CreateUserDto>>();
        _updateUseCaseMock = new Mock<IUpdateUseCase<UserDto, UpdateUserDto>>();
        _deleteUseCaseMock = new Mock<IDeleteUseCase<User, Guid>>();
        _usersController = new UsersController(
            _getAllUseCaseMock.Object,
            _getByIdUseCaseMock.Object,
            _createUseCaseMock.Object,
            _updateUseCaseMock.Object,
            _deleteUseCaseMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task GetAll_WhenExistingUsers_ShouldBeSuccess()
    {
        // Arrange
        var users = _fixture.CreateMany<UserDto>().ToList();
        _getAllUseCaseMock
            .Setup(x => x.ExecuteAsync())
            .ReturnsAsync(users);
        IEnumerable<UserDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.GetAll();
        
        // Assert
        await act.Should().NotThrowAsync();
        var userDtos = result.ToList();
        userDtos.Should().NotBeNull();
        userDtos.Should().BeEquivalentTo(users);
    }
    
    [Test]
    public async Task GetAll_WhenNotExistUsers_ShouldBeSuccess()
    {
        // Arrange
        var users = new List<UserDto>();
        _getAllUseCaseMock
            .Setup(x => x.ExecuteAsync())
            .ReturnsAsync(users);
        IEnumerable<UserDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.GetAll();
        
        // Assert
        await act.Should().NotThrowAsync();
        var userDtos = result.ToList();
        userDtos.Should().NotBeNull();
        userDtos.Should().BeEquivalentTo(users);
    }
    
    [Test]
    public async Task GetById_WhenExistingUser_ShouldBeSuccess()
    {
        // Arrange
        var user = _fixture.Create<UserDto>();
        _getByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        UserDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.GetById(user.Id);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }
    
    [Test]
    public async Task GetById_WhenException_ShouldThrow()
    {
        // Arrange
        _getByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
            .Throws<Exception>();
        UserDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.GetById(Guid.NewGuid());
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
        result.Should().BeNull();
    }
    
    [Test]
    public async Task Create_WhenValid_ShouldBeSuccess()
    {
        // Arrange
        var user = _fixture.Create<UserDto>();
        var createUser = _fixture.Create<CreateUserDto>();
        _createUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateUserDto>()))
            .ReturnsAsync(user);
        UserDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.Create(createUser);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }
    
    [Test]
    public async Task Create_WhenException_ShouldThrow()
    {
        // Arrange
        var createUser = _fixture.Create<CreateUserDto>();
        _createUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateUserDto>()))
            .Throws<Exception>();
        UserDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.Create(createUser);
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
        result.Should().BeNull();
    }
    
    [Test]
    public async Task Update_WhenValid_ShouldBeSuccess()
    {
        // Arrange
        var user = _fixture.Create<UserDto>();
        var updateUser = _fixture.Create<UpdateUserDto>();
        _updateUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<UpdateUserDto>()))
            .ReturnsAsync(user);
        UserDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.Update(updateUser);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }
    
    [Test]
    public async Task Update_WhenException_ShouldThrow()
    {
        // Arrange
        var updateUser = _fixture.Create<UpdateUserDto>();
        _updateUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<UpdateUserDto>()))
            .Throws<Exception>();
        UserDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _usersController.Update(updateUser);
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
        result.Should().BeNull();
    }
    
    [Test]
    public async Task Delete_WhenValid_ShouldBeSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        _deleteUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);
        
        // Act
        Func<Task> act = async () => await _usersController.Delete(id);
        
        // Assert
        await act.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task Delete_WhenException_ShouldThrow()
    {
        // Arrange
        var id = Guid.NewGuid();
        _deleteUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
            .Throws<Exception>();
        
        // Act
        Func<Task> act = async () => await _usersController.Delete(id);
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}