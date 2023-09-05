using System.Diagnostics.CodeAnalysis;
using Dapper;
using MovieApp.Adapter.Persistence.SqlServer.Factories.Interfaces;
using MovieApp.Core.Movies.Entities;
using MovieApp.Core.Ports.Persistence;

namespace MovieApp.Adapter.Persistence.SqlServer.Repositories;

[ExcludeFromCodeCoverage]
public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;

    }


    public async Task<Movie> GetAsync(Guid id)
    {
        const string sql = "SELECT * FROM Movies WHERE Id = @Id";
        using var connection = _dbConnectionFactory.Create();
        return await connection.QuerySingleOrDefaultAsync<Movie>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Movies";
        using var connection = _dbConnectionFactory.Create();
        return await connection.QueryAsync<Movie>(sql);
    }

    public async Task<Movie> CreateAsync(Movie movie)
    {
        const string sql = "INSERT INTO Movies (Id, Title, Description, ReleaseDate, Genre, CoverUrl, ImDbUrl, ImDbRating) VALUES (@Id, @Title, @Description, @ReleaseDate, @Genre, @CoverUrl, @ImDbUrl, @ImDbRating)";
        using var connection = _dbConnectionFactory.Create();
        await connection.ExecuteAsync(sql, movie);
        return movie;
    }

    public async Task<Movie> UpdateAsync(Movie movie)
    {
        const string sql = "UPDATE Movies SET Title = @Title, Description = @Description, ReleaseDate = @ReleaseDate, Genre = @Genre, CoverUrl = @CoverUrl, ImDbUrl = @ImDbUrl, ImDbRating = @ImDbRating WHERE Id = @Id";
        using var connection = _dbConnectionFactory.Create();
        await connection.ExecuteAsync(sql, movie);
        return movie;
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM Movies WHERE Id = @Id";
        using var connection = _dbConnectionFactory.Create();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}