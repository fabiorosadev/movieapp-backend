using System.Diagnostics.CodeAnalysis;
using MovieApp.Core.Abstractions.Exceptions;

namespace MovieApp.Core.Users.Exceptions;

[ExcludeFromCodeCoverage]
public class UserNotFoundException : BusinessException
{
    public UserNotFoundException(string message) : base(message)
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}