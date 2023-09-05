using AutoFixture;
using FluentAssertions;
using Moq;
using MovieApp.Core.Abstractions.Exceptions;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.UseCases;
using MovieApp.Core.Ports.Persistence;

namespace MovieApp.Core.Tests.Movies.UseCases;

public class CreateMovieUseCaseTest
{
    private Mock<IMovieRepository> _movieRepositoryMock = null!;
    private CreateMovieUseCase _createMovieUseCase = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _createMovieUseCase = new CreateMovieUseCase(_movieRepositoryMock.Object);
    }

    [Test]
    public async Task ExecuteAsync_WithValidCreateMovieDto_ShouldBeSuccess()
    {
        // Arrange
        var createMovieDto = _fixture.Create<CreateMovieDto>();
        MovieDto result = null!;
        _movieRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Movie>())).ReturnsAsync(createMovieDto.ToMovie());
        
        // Act
        Func<Task> act = async () => result = await _createMovieUseCase.ExecuteAsync(createMovieDto);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createMovieDto, options => options.ExcludingMissingMembers());
    }
    
    [Test]
    public async Task ExecuteAsync_WithInvalidCreateMovieDto_ShouldThrowValidationException()
    {
        // Arrange
        var createMovieDto = _fixture.Create<CreateMovieDto>();
        createMovieDto.Title = null!;
        MovieDto result = null!;
        
        // Act
        Func<Task> act = async () => result = await _createMovieUseCase.ExecuteAsync(createMovieDto);
        
        // Assert
        await act.Should().ThrowAsync<InputValidationException>();
        result.Should().BeNull();
    }
}