using System.Net;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.UseCases.Interfaces;
using MovieApp.Host.WebApi.Authorization.Interfaces;
using MovieApp.Host.WebApi.Controllers;
using MovieApp.Host.WebApi.Models.Authentication;

namespace MovieApp.Host.WebApi.Tests.Controllers;

public class AuthenticationControllerTest
{
    private Mock<IAuthenticateUserUseCase> _authenticateUserUseCaseMock = null!;
    private Mock<IJwtUtils> _jwtUtilsMock = null!;
    private AuthenticationController _authenticationController = null!;
    private Fixture _fixture = null!;
    
    [SetUp]
    public void Setup()
    {
        _authenticateUserUseCaseMock = new Mock<IAuthenticateUserUseCase>();
        _jwtUtilsMock = new Mock<IJwtUtils>();
        _authenticationController = new AuthenticationController(_authenticateUserUseCaseMock.Object, _jwtUtilsMock.Object);
        _fixture = new Fixture();
    }
    
    [Test]
    public async Task Authenticate_WhenUserIsAuthenticated_ReturnsOk()
    {
        // Arrange
        var userDto = _fixture.Create<UserDto>();
        var token = _fixture.Create<string>();
        var authenticationRequestModel = _fixture.Create<AuthenticationRequestModel>();
        
        _authenticateUserUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<AuthUserDto>()))
            .ReturnsAsync(userDto);
        
        _jwtUtilsMock
            .Setup(x => x.GenerateJwtToken(It.IsAny<AuthUser>()))
            .Returns(token);
        IActionResult result = null!;
        
        // Act
        Func<Task> act = async () =>  result = await _authenticationController.Authenticate(authenticationRequestModel);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var okObjectResult = result as OkObjectResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult?.Value.Should().NotBeNull();
        okObjectResult?.Value.Should().BeOfType<AuthenticationResponseModel>();
        var authenticationResponseModelResult = okObjectResult?.Value as AuthenticationResponseModel;
        authenticationResponseModelResult.Should().NotBeNull();
        authenticationResponseModelResult?.Token.Should().NotBeNull();
        authenticationResponseModelResult?.Token.Should().Be(token);
        _jwtUtilsMock.Verify(v => v.GenerateJwtToken(It.Is<AuthUser>(args => args.Id.Equals(userDto.Id))));
    }
    
    [Test]
    public async Task Authenticate_WhenUserIsNotAuthenticated_ReturnsBadRequest()
    {
        // Arrange
        var authenticationRequestModel = _fixture.Create<AuthenticationRequestModel>();
        var userDto = _fixture.Create<UserDto>();
        IActionResult result = null!;
        
        _authenticateUserUseCaseMock
            .Setup(x => x.ExecuteAsync(It.IsAny<AuthUserDto>()))
            .ReturnsAsync((UserDto)null!);
        
        // Act
        Func<Task> act = async () =>  result = await _authenticationController.Authenticate(authenticationRequestModel);
        
        // Assert
        await act.Should().NotThrowAsync();
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestObjectResult = result as BadRequestObjectResult;
        badRequestObjectResult.Should().NotBeNull();
        badRequestObjectResult?.Value.Should().NotBeNull();
        badRequestObjectResult?.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}