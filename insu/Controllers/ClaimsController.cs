using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using insu.Data;
using insu.Models;

namespace insu.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClaimsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClaimsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<insu.Models.Claim>>> GetClaims()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = _context.Claims.Include(c => c.User).Include(c => c.PolicyApplication).ThenInclude(pa => pa.Policy);

        if (User.IsInRole("Agent"))
            return await query.Where(c => c.PolicyApplication.AgentId == userId).ToListAsync();
        else
            return await query.Where(c => c.UserId == userId).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<insu.Models.Claim>> GetClaim(int id)
    {
        var claim = await _context.Claims.Include(c => c.User).Include(c => c.PolicyApplication).ThenInclude(pa => pa.Policy).FirstOrDefaultAsync(c => c.Id == id);
        if (claim == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Admin") && claim.UserId != userId)
            return Forbid();

        return claim;
    }

    [HttpPost]
    public async Task<ActionResult<insu.Models.Claim>> CreateClaim(CreateClaimDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Check if user has approved application for this policy
        var approvedApp = await _context.PolicyApplications
            .FirstOrDefaultAsync(pa => pa.Id == dto.PolicyApplicationId && pa.UserId == userId && pa.Status == "Approved");

        if (approvedApp == null)
            return BadRequest("You can only claim on approved policies");

        var claim = new insu.Models.Claim
        {
            Title = dto.Title,
            Description = dto.Description,
            Amount = dto.Amount,
            UserId = userId,
            PolicyApplicationId = dto.PolicyApplicationId
        };

        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetClaim), new { id = claim.Id }, claim);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClaim(int id, insu.Models.Claim claim)
    {
        if (id != claim.Id) return BadRequest();

        var existingClaim = await _context.Claims.FindAsync(id);
        if (existingClaim == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Admin") && existingClaim.UserId != userId)
            return Forbid();

        claim.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        _context.Entry(existingClaim).CurrentValues.SetValues(claim);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClaim(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Admin") && claim.UserId != userId)
            return Forbid();

        _context.Claims.Remove(claim);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> ApproveClaim(int id, ClaimApprovalDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var claim = await _context.Claims.Include(c => c.PolicyApplication).FirstOrDefaultAsync(c => c.Id == id);
        if (claim == null) return NotFound();

        if (claim.PolicyApplication.AgentId != userId)
            return Forbid();

        claim.Status = dto.Status;
        claim.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("user-approved-policies")]
    public async Task<ActionResult<IEnumerable<PolicyApplication>>> GetUserApprovedPolicies()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var approvedPolicies = await _context.PolicyApplications
            .Include(pa => pa.Policy)
            .Where(pa => pa.UserId == userId && pa.Status == "Approved")
            .ToListAsync();
        return approvedPolicies;
    }
}

public record CreateClaimDto(string Title, string Description, decimal Amount, int PolicyApplicationId);
public record ClaimApprovalDto(string Status);