using System.Diagnostics.CodeAnalysis;
using Dapper;
using MovieApp.Adapter.Persistence.SqlServer.Factories.Interfaces;
using MovieApp.Core.Ports.Persistence;
using MovieApp.Core.Users.Entities;

namespace MovieApp.Adapter.Persistence.SqlServer.Repositories;

[ExcludeFromCodeCoverage]
public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<User> GetAsync(Guid id)
    {
        const string sql = "SELECT * FROM Users WHERE Id = @Id";
        using var connection = _dbConnectionFactory.Create();
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string sql = "SELECT * FROM Users";
        using var connection = _dbConnectionFactory.Create();
        return await connection.QueryAsync<User>(sql);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = "SELECT * FROM Users WHERE Email = @Email";
        using var connection = _dbConnectionFactory.Create();
        return await connection.QuerySingleOrDefaultAsync<User?>(sql, new { Email = email });
    }

    public async Task<User> CreateAsync(User user)
    {
        const string sql = "INSERT INTO Users (Id, Email, PasswordHash, FirstName, LastName, Role) VALUES (@Id, @Email, @PasswordHash, @FirstName, @LastName, @Role)";
        using var connection = _dbConnectionFactory.Create();
        await connection.ExecuteAsync(sql, user);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        const string sql = "UPDATE Users SET Email = @Email, PasswordHash = @PasswordHash, FirstName = @FirstName, LastName = @LastName, Role = @Role WHERE Id = @Id";
        using var connection = _dbConnectionFactory.Create();
        await connection.ExecuteAsync(sql, user);
        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM Users WHERE Id = @Id";
        using var connection = _dbConnectionFactory.Create();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}