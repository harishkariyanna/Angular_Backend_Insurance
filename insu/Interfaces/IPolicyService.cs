using insu.Models;

namespace insu.Interfaces;

public interface IPolicyService
{
    Task<IEnumerable<Policy>> GetAllPoliciesAsync();
    Task<Policy?> GetPolicyByIdAsync(int id);
    Task<Policy> CreatePolicyAsync(Policy policy);
    Task UpdatePolicyAsync(Policy policy);
    Task DeletePolicyAsync(int id);
}