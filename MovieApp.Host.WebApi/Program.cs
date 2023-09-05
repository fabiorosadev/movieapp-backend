using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using MovieApp.Adapter.Persistence.SqlServer;
using MovieApp.Core.Abstractions.UseCases.Interfaces;
using MovieApp.Core.Movies.Dtos;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Movies.UseCases;
using MovieApp.Core.Users.Dtos;
using MovieApp.Core.Users.Entities;
using MovieApp.Core.Users.UseCases;
using MovieApp.Core.Users.UseCases.Interfaces;
using MovieApp.Host.WebApi.Authorization;
using MovieApp.Host.WebApi.Authorization.Interfaces;
using MovieApp.Host.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var persistenceAdapter = new PersistenceAdapter(new PersistenceAdapterSettings()
{
    ConnectionString = builder.Configuration.GetConnectionString("MovieApp")!
});
await persistenceAdapter.Initialize();
persistenceAdapter.Register(builder.Services);

builder.Services.AddSingleton(x => builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!);
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddScoped<IGetAllUseCase<MovieDto>, GetAllMoviesUseCase>();
builder.Services.AddScoped<IGetByIdUseCase<MovieDto, Guid>, GetMovieUseCase>();
builder.Services.AddScoped<ICreateUseCase<MovieDto, CreateMovieDto>, CreateMovieUseCase>();
builder.Services.AddScoped<IUpdateUseCase<MovieDto, UpdateMovieDto>, UpdateMovieUseCase>();
builder.Services.AddScoped<IDeleteUseCase<Movie, Guid>, DeleteMovieUseCase>();

builder.Services.AddScoped<IGetAllUseCase<UserDto>, GetAllUsersUseCase>();
builder.Services.AddScoped<IGetByIdUseCase<UserDto, Guid>, GetUserByIdUseCase>();
builder.Services.AddScoped<IGetUserByEmailUseCase, GetUserByEmailUseCase>();
builder.Services.AddScoped<ICreateUseCase<UserDto, CreateUserDto>, CreateUserUseCase>();
builder.Services.AddScoped<IUpdateUseCase<UserDto, UpdateUserDto>, UpdateUserUseCase>();
builder.Services.AddScoped<IDeleteUseCase<User, Guid>, DeleteUserUseCase>();
builder.Services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program {}