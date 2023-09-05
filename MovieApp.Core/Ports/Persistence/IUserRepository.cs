using MovieApp.Core.Users.Entities;

namespace MovieApp.Core.Ports.Persistence;

public interface IUserRepository
{
    Task<User> GetAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}