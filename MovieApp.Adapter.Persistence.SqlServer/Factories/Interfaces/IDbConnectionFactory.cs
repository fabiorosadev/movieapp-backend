using System.Data;

namespace MovieApp.Adapter.Persistence.SqlServer.Factories.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}