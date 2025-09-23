using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using insu.Data;
using insu.Models;

namespace insu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoliciesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PoliciesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Policy>>> GetPolicies()
    {
        return await _context.Policies.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Policy>> GetPolicy(int id)
    {
        var policy = await _context.Policies.FindAsync(id);
        return policy == null ? NotFound() : policy;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Policy>> CreatePolicy(Policy policy)
    {
        _context.Policies.Add(policy);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPolicy), new { id = policy.Id }, policy);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePolicy(int id, Policy policy)
    {
        if (id != policy.Id) return BadRequest();

        policy.UpdatedAt = DateTime.UtcNow;
        _context.Entry(policy).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Policies.AnyAsync(e => e.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePolicy(int id)
    {
        var policy = await _context.Policies.FindAsync(id);
        if (policy == null) return NotFound();

        _context.Policies.Remove(policy);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}