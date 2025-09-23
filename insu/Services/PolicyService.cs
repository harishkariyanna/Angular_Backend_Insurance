using insu.Interfaces;
using insu.Models;

namespace insu.Services;

public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository _policyRepository;

    public PolicyService(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        return await _policyRepository.GetAllAsync();
    }

    public async Task<Policy?> GetPolicyByIdAsync(int id)
    {
        return await _policyRepository.GetByIdAsync(id);
    }

    public async Task<Policy> CreatePolicyAsync(Policy policy)
    {
        return await _policyRepository.CreateAsync(policy);
    }

    public async Task UpdatePolicyAsync(Policy policy)
    {
        await _policyRepository.UpdateAsync(policy);
    }

    public async Task DeletePolicyAsync(int id)
    {
        await _policyRepository.DeleteAsync(id);
    }
}