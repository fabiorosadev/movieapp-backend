using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.UseCases;
using MovieApp.Core.Ports.Persistence;

namespace MovieApp.Core.Tests.Movies.UseCases;

public class UpdateMovieUseCaseTest
{
    private Mock<IMovieRepository> _movieRepositoryMock = null!;
    private UpdateMovieUseCase _updateMovieUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _updateMovieUseCase = new UpdateMovieUseCase(_movieRepositoryMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task ExecuteAsync_WithValidUpdateMovieDto_ShouldBeSuccess()
    {
        // Arrange
        var updateMovieDto = _fixture.Create<UpdateMovieDto>();
        MovieDto result = null!;
        _movieRepositoryMock.Setup(x => x.GetAsync(updateMovieDto.Id)).ReturnsAsync(updateMovieDto.ToMovie());
        _movieRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Movie>())).ReturnsAsync(updateMovieDto.ToMovie());
        
        // Act
        Func<Task> act = async () => result = await _updateMovieUseCase.ExecuteAsync(updateMovieDto);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updateMovieDto, options => options.ExcludingMissingMembers());
    }
    
    [Test]
    public async Task ExecuteAsync_WithInvalidUpdateMovieDto_ShouldThrowValidationException()
    {
        // Arrange
        var updateMovieDto = _fixture.Create<UpdateMovieDto>();
        updateMovieDto.Title = null!;
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _updateMovieUseCase.ExecuteAsync(updateMovieDto);
        
        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
        result.Should().BeNull();
    }
    
    [Test]
    public async Task ExecuteAsync_WithNonExistingId_ShouldThrowNotFoundException()
    {
        // Arrange
        var updateMovieDto = _fixture.Create<UpdateMovieDto>();
        Movie movie = null!;
        _movieRepositoryMock.Setup(x => x.GetAsync(updateMovieDto.Id)).ReturnsAsync(movie);
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _updateMovieUseCase.ExecuteAsync(updateMovieDto);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}