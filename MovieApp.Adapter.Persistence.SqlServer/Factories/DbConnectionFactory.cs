using System.Data;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using MovieApp.Adapter.Persistence.SqlServer.Factories.Interfaces;

namespace MovieApp.Adapter.Persistence.SqlServer.Factories;

[ExcludeFromCodeCoverage]
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));

        _connectionString = connectionString;
    }
    
    public IDbConnection Create()
    {
        var connString =  new SqlConnection(_connectionString);
        return connString;
    }
}