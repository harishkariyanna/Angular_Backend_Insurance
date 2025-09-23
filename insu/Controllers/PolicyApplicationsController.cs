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
public class PolicyApplicationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PolicyApplicationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PolicyApplication>>> GetApplications()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");

        var query = _context.PolicyApplications
            .Include(a => a.User)
            .Include(a => a.Policy)
            .Include(a => a.AssignedAgent);
        
        if (User.IsInRole("Admin"))
            return await query.ToListAsync();
        else if (User.IsInRole("Agent"))
            return await query.Where(a => a.AgentId == userId).ToListAsync();
        else
            return await query.Where(a => a.UserId == userId).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<PolicyApplication>> CreateApplication(PolicyApplicationDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        // Find if user has an assigned agent
        var existingAssignment = await _context.PolicyApplications
            .Where(pa => pa.UserId == userId && pa.AgentId != null)
            .FirstOrDefaultAsync();

        var application = new PolicyApplication
        {
            UserId = userId,
            PolicyId = dto.PolicyId,
            DurationMonths = dto.DurationMonths,
            AgentId = existingAssignment?.AgentId
        };

        _context.PolicyApplications.Add(application);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PolicyApplication>> GetApplication(int id)
    {
        var application = await _context.PolicyApplications
            .Include(a => a.User).Include(a => a.Policy).Include(a => a.AssignedAgent)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Admin") && application.UserId != userId)
            return Forbid();

        return application;
    }

    [HttpPut("{id}/assign-agent")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult > AssignAgent(int id, AssignAgentDto dto)
    {
        var application = await _context.PolicyApplications.FindAsync(id);
        if (application == null) return NotFound();

        application.AgentId = dto.AgentId;
        application.Status = "Assigned";
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> ApproveApplication(int id, ApprovalDto dto)
    {
        var application = await _context.PolicyApplications.FindAsync(id);
        if (application == null) return NotFound();

        application.Status = dto.Status;
        application.AdminComments = dto.Comments;
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("agent-customers")]
    [Authorize(Roles = "Agent")]
    public async Task<ActionResult<IEnumerable<PolicyApplication>>> GetAgentCustomers()
    {
        var agentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var applications = await _context.PolicyApplications
            .Include(a => a.User)
            .Include(a => a.Policy)
            .Where(a => a.AgentId == agentId)
            .ToListAsync();
        return applications;
    }

    [HttpGet("agents")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAgents()
    {
        var agents = await _context.Users.Where(u => u.Role == "Agent").ToListAsync();
        return agents;
    }
}

public record PolicyApplicationDto(int PolicyId, int DurationMonths);
public record ApprovalDto(string Status, string? Comments);
public record AssignAgentDto(int AgentId);