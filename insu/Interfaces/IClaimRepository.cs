using insu.Models;

namespace insu.Interfaces;

public interface IClaimRepository
{
    Task<IEnumerable<Claim>> GetAllAsync();
    Task<IEnumerable<Claim>> GetByUserIdAsync(int userId);
    Task<Claim?> GetByIdAsync(int id);
    Task<Claim> CreateAsync(Claim claim);
    Task UpdateAsync(Claim claim);
}