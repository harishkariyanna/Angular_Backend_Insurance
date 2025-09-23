using Microsoft.EntityFrameworkCore;
using insu.Data;
using insu.Interfaces;
using insu.Models;

namespace insu.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly AppDbContext _context;

    public PolicyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Policy>> GetAllAsync()
    {
        return await _context.Policies.ToListAsync();
    }

    public async Task<Policy?> GetByIdAsync(int id)
    {
        return await _context.Policies.FindAsync(id);
    }

    public async Task<Policy> CreateAsync(Policy policy)
    {
        _context.Policies.Add(policy);
        await _context.SaveChangesAsync();
        return policy;
    }

    public async Task UpdateAsync(Policy policy)
    {
        _context.Policies.Update(policy);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var policy = await _context.Policies.FindAsync(id);
        if (policy != null)
        {
            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();
        }
    }
}