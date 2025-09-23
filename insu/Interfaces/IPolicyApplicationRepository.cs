using insu.Models;

namespace insu.Interfaces;

public interface IPolicyApplicationRepository
{
    Task<IEnumerable<PolicyApplication>> GetAllAsync();
    Task<IEnumerable<PolicyApplication>> GetByUserIdAsync(int userId);
    Task<IEnumerable<PolicyApplication>> GetApprovedByUserIdAsync(int userId);
    Task<PolicyApplication?> GetByIdAsync(int id);
    Task<PolicyApplication> CreateAsync(PolicyApplication application);
    Task UpdateAsync(PolicyApplication application);
}