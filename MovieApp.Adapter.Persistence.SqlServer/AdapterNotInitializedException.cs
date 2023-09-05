using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MovieApp.Adapter.Persistence.SqlServer;

[ExcludeFromCodeCoverage]
public class AdapterNotInitializedException : Exception
{
    public AdapterNotInitializedException()
    {
        
    }
    
    public AdapterNotInitializedException(string message) : base(message)
    {
        
    }
    
    public AdapterNotInitializedException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
    
    protected AdapterNotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}