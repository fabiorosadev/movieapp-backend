using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.UseCases;
using MovieApp.Core.Ports.Persistence;

namespace MovieApp.Core.Tests.Movies.UseCases;

public class GetMovieUseCaseTest
{
    private Mock<IMovieRepository> _movieRepositoryMock = null!;
    private GetMovieUseCase _getMovieUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _getMovieUseCase = new GetMovieUseCase(_movieRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WhenMovieExists_ReturnsMovie()
    {
        // Arrange
        var movie = _fixture.Create<Movie>();
        _movieRepositoryMock.Setup(x => x.GetAsync(movie.Id)).ReturnsAsync(movie);
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getMovieUseCase.ExecuteAsync(movie.Id);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(movie, options => options.ExcludingMissingMembers());
    }
    
    [Test]
    public async Task ExecuteAsync_WhenMovieDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var movieId = _fixture.Create<Guid>();
        Movie movie = null!;
        _movieRepositoryMock.Setup(x => x.GetAsync(movieId)).ReturnsAsync(movie);
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _getMovieUseCase.ExecuteAsync(movieId);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}