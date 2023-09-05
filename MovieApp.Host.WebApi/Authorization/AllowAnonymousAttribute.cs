using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Host.WebApi.Authorization;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{
    
}