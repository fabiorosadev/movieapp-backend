using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.UseCases;
using MovieApp.Core.Ports.Persistence;

namespace MovieApp.Core.Tests.Movies.UseCases;

public class DeleteMovieUseCaseTest
{
    private Mock<IMovieRepository> _movieRepositoryMock = null!;
    private DeleteMovieUseCase _deleteMovieUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _deleteMovieUseCase = new DeleteMovieUseCase(_movieRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WithValidId_ShouldBeSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        Movie movie = _fixture.Create<Movie>();
        _movieRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(movie);
        
        // Act
        Func<Task> act = async () => await _deleteMovieUseCase.ExecuteAsync(id);
        
        // Assert
        await act.Should().NotThrowAsync();
        _movieRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
    }
    
    [Test]
    public async Task ExecuteAsync_WithInvalidId_ShouldThrowValidationException()
    {
        // Arrange
        var id = Guid.Empty;
        
        // Act
        Func<Task> act = async () => await _deleteMovieUseCase.ExecuteAsync(id);
        
        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
    }
    
    [Test]
    public async Task ExecuteAsync_WithNonExistingId_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        Movie movie = null!;
        _movieRepositoryMock.Setup(x => x.GetAsync(id)).ReturnsAsync(movie);
        
        // Act
        Func<Task> act = async () => await _deleteMovieUseCase.ExecuteAsync(id);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}