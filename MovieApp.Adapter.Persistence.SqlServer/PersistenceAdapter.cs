using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.Adapter.Persistence.SqlServer.Factories;
using MovieApp.Adapter.Persistence.SqlServer.Factories.Interfaces;
using MovieApp.Adapter.Persistence.SqlServer.Repositories;
using MovieApp.Core.Ports.Persistence;

namespace MovieApp.Adapter.Persistence.SqlServer;

[ExcludeFromCodeCoverage]
public class PersistenceAdapter
{
    private readonly PersistenceAdapterSettings _settings;
    private bool _initialized;
    
    public PersistenceAdapter(PersistenceAdapterSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }
    
    public async Task Initialize()
    {
        await PersistenceAdapterInitialization.InitializeTablesAsync(_settings.ConnectionString);
        _initialized = true;
    }
    
    public void Register(IServiceCollection services)
    {
        if (!_initialized)
            throw new AdapterNotInitializedException();
        
        var dbConnectionFactory = new DbConnectionFactory(_settings.ConnectionString);
        services.AddSingleton<IDbConnectionFactory>(dbConnectionFactory);
        services.AddSingleton<IMovieRepository, MovieRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
    }
}