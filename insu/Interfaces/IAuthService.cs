using insu.Models;

namespace insu.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task<User> RegisterAsync(string email, string password);
    string GenerateJwtToken(User user);
}