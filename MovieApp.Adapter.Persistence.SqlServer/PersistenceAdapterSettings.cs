using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Adapter.Persistence.SqlServer;

[ExcludeFromCodeCoverage]
public class PersistenceAdapterSettings
{
    public string ConnectionString { get; set; } = null!;
}