using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Core.Abstractions.Exceptions;

[ExcludeFromCodeCoverage]
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
    
    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}