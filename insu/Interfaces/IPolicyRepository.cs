using insu.Models;

namespace insu.Interfaces;

public interface IPolicyRepository
{
    Task<IEnumerable<Policy>> GetAllAsync();
    Task<Policy?> GetByIdAsync(int id);
    Task<Policy> CreateAsync(Policy policy);
    Task UpdateAsync(Policy policy);
    Task DeleteAsync(int id);
}