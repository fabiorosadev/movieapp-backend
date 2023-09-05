using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Core.Abstractions.Exceptions;

[ExcludeFromCodeCoverage]
public class InputValidationException : Exception
{
    public InputValidationException(string message) : base(message)
    {
    }
    
    public InputValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}