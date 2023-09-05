using System.Diagnostics.CodeAnalysis;

namespace MovieApp.Host.WebApi.Authorization;

[ExcludeFromCodeCoverage]
public class JwtSettings
{
    public string Secret { get; set; }
}