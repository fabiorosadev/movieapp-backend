using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Movies.Entities;
using MovieApp.Host.WebApi.Controllers;

namespace MovieApp.Host.WebApi.Tests.Controllers;

public class MoviesControllerTest
{
    private Mock<IGetAllUseCase<MovieDto>> _getAllUseCaseMock = null!;
    private Mock<IGetByIdUseCase<MovieDto, Guid>> _getByIdUseCaseMock = null!;
    private Mock<ICreateUseCase<MovieDto, CreateMovieDto>> _createUseCaseMock = null!;
    private Mock<IUpdateUseCase<MovieDto, UpdateMovieDto>> _updateUseCaseMock = null!;
    private Mock<IDeleteUseCase<Movie, Guid>> _deleteUseCaseMock = null!;
    private MoviesController _moviesController = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _getAllUseCaseMock = new Mock<IGetAllUseCase<MovieDto>>();
        _getByIdUseCaseMock = new Mock<IGetByIdUseCase<MovieDto, Guid>>();
        _createUseCaseMock = new Mock<ICreateUseCase<MovieDto, CreateMovieDto>>();
        _updateUseCaseMock = new Mock<IUpdateUseCase<MovieDto, UpdateMovieDto>>();
        _deleteUseCaseMock = new Mock<IDeleteUseCase<Movie, Guid>>();
        _moviesController = new MoviesController(
            _getAllUseCaseMock.Object,
            _getByIdUseCaseMock.Object,
            _createUseCaseMock.Object,
            _updateUseCaseMock.Object,
            _deleteUseCaseMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task GetAll_WhenExistingMovies_ShouldBeSuccess()
    {
        // Arrange
        var movies = _fixture.CreateMany<MovieDto>().ToList();
        _getAllUseCaseMock
            .Setup(x => x.ExecuteAsync())
            .ReturnsAsync(movies);
        IEnumerable<MovieDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.GetAll();
        
        // Assert
        await act.Should().NotThrowAsync();
        var movieDtos = result.ToList();
        movieDtos.Should().NotBeNull();
        movieDtos.Should().BeEquivalentTo(movies);
    }

    [Test]
    public async Task GetAll_WhenNotExistMovies_ShouldBeSuccess()
    {
        // Arrange
        List<MovieDto> movies = null!;
        movies = new List<MovieDto>();
        _getAllUseCaseMock
            .Setup(x => x.ExecuteAsync())
            .ReturnsAsync(movies);
        IEnumerable<MovieDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.GetAll();
        
        // Assert
        await act.Should().NotThrowAsync();
        var movieDtos = result.ToList();
        movieDtos.Should().NotBeNull();
        movieDtos.Should().BeEmpty();
    }
    
    [Test]
    public async Task GetById_WhenExistingMovie_ShouldBeSuccess()
    {
        // Arrange
        var movie = _fixture.Create<MovieDto>();
        _getByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(movie);
        MovieDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.GetById(Guid.NewGuid());
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(movie);
    }
    
    [Test]
    public async Task GetById_WhenException_ShouldThrow()
    {
        // Arrange
        _getByIdUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
            .Throws<Exception>();
        MovieDto? result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.GetById(Guid.NewGuid());
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
        result.Should().BeNull();
    }
    
    [Test]
    public async Task Create_WhenValid_ShouldBeSuccess()
    {
        // Arrange
        var createMovie = _fixture.Create<CreateMovieDto>();
        var movie = _fixture.Create<MovieDto>();
        _createUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateMovieDto>()))
            .ReturnsAsync(movie);
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.Create(createMovie);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(movie);
    }
    
    [Test]
    public async Task Create_WhenException_ShouldThrow()
    {
        // Arrange
        var createMovie = _fixture.Create<CreateMovieDto>();
        _createUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<CreateMovieDto>()))
            .Throws<Exception>();
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.Create(createMovie);
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
        result.Should().BeNull();
    }
    
    [Test]
    public async Task Update_WhenValid_ShouldBeSuccess()
    {
        // Arrange
        var updateMovie = _fixture.Create<UpdateMovieDto>();
        var movie = _fixture.Create<MovieDto>();
        _updateUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<UpdateMovieDto>()))
            .ReturnsAsync(movie);
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.Update(updateMovie);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(movie);
    }
    
    [Test]
    public async Task Update_WhenException_ShouldThrow()
    {
        // Arrange
        var updateMovie = _fixture.Create<UpdateMovieDto>();
        _updateUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<UpdateMovieDto>()))
            .Throws<Exception>();
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _moviesController.Update(updateMovie);
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
        result.Should().BeNull();
    }
    
    [Test]
    public async Task Delete_WhenValid_ShouldBeSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        Func<Task> act = async () => await _moviesController.Delete(id);
        
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
        Func<Task> act = async () => await _moviesController.Delete(id);
        
        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}