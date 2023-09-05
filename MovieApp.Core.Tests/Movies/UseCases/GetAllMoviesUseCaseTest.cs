using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.UseCases;
using MovieApp.Core.Ports.Persistence;

namespace MovieApp.Core.Tests.Movies.UseCases;

public class GetAllMoviesUseCaseTest
{
    private Mock<IMovieRepository> _movieRepositoryMock = null!;
    private GetAllMoviesUseCase _getAllMoviesUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _getAllMoviesUseCase = new GetAllMoviesUseCase(_movieRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WhenMoviesExist_ReturnsMovies()
    {
        // Arrange
        var movies = _fixture.CreateMany<Movie>().ToList();
        _movieRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(movies);
        IEnumerable<MovieDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getAllMoviesUseCase.ExecuteAsync();
        
        // Assert
        await act.Should().NotThrowAsync();
        var movieDtos = result.ToList();
        movieDtos.Should().NotBeNullOrEmpty();
        movieDtos.Should().BeEquivalentTo(movies, options => options.ExcludingMissingMembers());
    }
    
    [Test]
    public async Task ExecuteAsync_WhenMoviesDoNotExist_ReturnsEmptyList()
    {
        // Arrange
        IEnumerable<Movie> movies = new List<Movie>();
        _movieRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(movies);
        IEnumerable<MovieDto> result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getAllMoviesUseCase.ExecuteAsync();
        
        // Assert
        await act.Should().NotThrowAsync();
        var movieDtos = result.ToList();
        movieDtos.Should().BeEmpty();
    }
}