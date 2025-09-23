using Microsoft.EntityFrameworkCore;
using insu.Data;
using insu.Interfaces;
using insu.Models;

namespace insu.Repositories;

public class ClaimRepository : IClaimRepository
{
    private readonly AppDbContext _context;

    public ClaimRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Claim>> GetAllAsync()
    {
        return await _context.Claims
            .Include(c => c.User)
            .Include(c => c.PolicyApplication)
            .ThenInclude(pa => pa.Policy)
            .ToListAsync();
    }

    public async Task<IEnumerable<Claim>> GetByUserIdAsync(int userId)
    {
        return await _context.Claims
            .Include(c => c.User)
            .Include(c => c.PolicyApplication)
            .ThenInclude(pa => pa.Policy)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<Claim?> GetByIdAsync(int id)
    {
        return await _context.Claims
            .Include(c => c.User)
            .Include(c => c.PolicyApplication)
            .ThenInclude(pa => pa.Policy)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Claim> CreateAsync(Claim claim)
    {
        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();
        return claim;
    }

    public async Task UpdateAsync(Claim claim)
    {
        _context.Claims.Update(claim);
        await _context.SaveChangesAsync();
    }
}