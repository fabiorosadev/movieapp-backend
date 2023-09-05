using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Core.Abstractions.Exceptions;

[ExcludeFromCodeCoverage]
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
    
    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}