using System.Diagnostics.CodeAnalysis;
using Dapper;
using Microsoft.Data.SqlClient;
using MovieApp.Core.Users.Entities;

namespace MovieApp.Adapter.Persistence.SqlServer;

[ExcludeFromCodeCoverage]
public static class PersistenceAdapterInitialization
{
    public static async Task InitializeTablesAsync(string connectionString)
    {
        await using var connection = new SqlConnection(connectionString);
        await InitUsers();
        await InitMovies();
        await InitAdminUser();
        
        return;

        async Task InitUsers()
        {
            const string sql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users')
                CREATE TABLE Users (
                    Id uniqueidentifier PRIMARY KEY,
                    FirstName nvarchar(50) NOT NULL,
                    LastName nvarchar(50) NOT NULL,
                    Email nvarchar(256) NOT NULL,
                    PasswordHash nvarchar(256) NOT NULL,
                    Role nvarchar(50) NOT NULL                    
                )";

            await connection.ExecuteAsync(sql);
        }
        
        async Task InitMovies()
        {
            const string sql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Movies')
                CREATE TABLE Movies (
                    Id uniqueidentifier PRIMARY KEY,
                    Title nvarchar(50) NOT NULL,
                    Description nvarchar(256) NOT NULL,
                    ReleaseDate datetime NOT NULL,
                    Genre int NOT NULL,
                    CoverUrl nvarchar(256) NOT NULL,
                    ImDbUrl nvarchar(256) NOT NULL,
                    ImDbRating decimal(3,1) NOT NULL,
                )";

            await connection.ExecuteAsync(sql);
        }

        async Task InitAdminUser()
        {
            const string sql = @"
                IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'admin@admin.com')
                INSERT INTO Users (Id, FirstName, LastName, Email, PasswordHash, Role) VALUES (@Id, @FirstName, @LastName, @Email, @PasswordHash, @Role)";
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@admin.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin1234"),
            };

            await connection.ExecuteAsync(sql, adminUser);
        }
    }
}