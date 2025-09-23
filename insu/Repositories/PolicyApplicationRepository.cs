using Microsoft.EntityFrameworkCore;
using insu.Data;
using insu.Interfaces;
using insu.Models;

namespace insu.Repositories;

public class PolicyApplicationRepository : IPolicyApplicationRepository
{
    private readonly AppDbContext _context;

    public PolicyApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PolicyApplication>> GetAllAsync()
    {
        return await _context.PolicyApplications
            .Include(pa => pa.User)
            .Include(pa => pa.Policy)
            .ToListAsync();
    }

    public async Task<IEnumerable<PolicyApplication>> GetByUserIdAsync(int userId)
    {
        return await _context.PolicyApplications
            .Include(pa => pa.User)
            .Include(pa => pa.Policy)
            .Where(pa => pa.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<PolicyApplication>> GetApprovedByUserIdAsync(int userId)
    {
        return await _context.PolicyApplications
            .Include(pa => pa.Policy)
            .Where(pa => pa.UserId == userId && pa.Status == "Approved")
            .ToListAsync();
    }

    public async Task<PolicyApplication?> GetByIdAsync(int id)
    {
        return await _context.PolicyApplications
            .Include(pa => pa.User)
            .Include(pa => pa.Policy)
            .FirstOrDefaultAsync(pa => pa.Id == id);
    }

    public async Task<PolicyApplication> CreateAsync(PolicyApplication application)
    {
        _context.PolicyApplications.Add(application);
        await _context.SaveChangesAsync();
        return application;
    }

    public async Task UpdateAsync(PolicyApplication application)
    {
        _context.PolicyApplications.Update(application);
        await _context.SaveChangesAsync();
    }
}