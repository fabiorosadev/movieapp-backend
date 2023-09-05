using System.Text.Json.Serialization;
using MovieApp.Core.Enums;

namespace MovieApp.Core.Users.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Role Role { get; set; }
    
    [JsonIgnore]
    public string? PasswordHash { get; set; }
}