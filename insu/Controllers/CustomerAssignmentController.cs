using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using insu.Data;
using insu.Models;

namespace insu.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomerAssignmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerAssignmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("customers")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _context.Users
            .Where(u => u.Role == "User")
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Email,
                c.CreatedAt,
                AssignedAgent = _context.PolicyApplications
                    .Where(pa => pa.UserId == c.Id && pa.AgentId != null)
                    .Select(pa => pa.AssignedAgent.Name)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(customers);
    }

    [HttpPost("assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignCustomerToAgent([FromBody] AssignCustomerDto dto)
    {
        var customer = await _context.Users.FindAsync(dto.CustomerId);
        var agent = await _context.Users.FindAsync(dto.AgentId);

        if (customer == null || customer.Role != "User")
            return BadRequest("Customer not found");

        if (agent == null || agent.Role != "Agent")
            return BadRequest("Agent not found");

        // Update all policy applications for this customer to the new agent
        var policyApplications = await _context.PolicyApplications
            .Where(pa => pa.UserId == dto.CustomerId)
            .ToListAsync();

        foreach (var pa in policyApplications)
        {
            pa.AgentId = dto.AgentId;
            pa.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Customer assigned to agent successfully" });
    }
}

public record AssignCustomerDto(int CustomerId, int AgentId);