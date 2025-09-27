using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using insu.Data;
using insu.Models;

namespace insu.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AgentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AgentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAgents()
    {
        var agents = await _context.Users
            .Where(u => u.Role == "Agent")
            .Select(a => new
            {
                a.Id,
                a.Name,
                a.Email,
                a.CreatedAt,
                CustomerCount = _context.PolicyApplications.Count(pa => pa.AgentId == a.Id)
            })
            .ToListAsync();

        return Ok(agents);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAgent([FromBody] CreateAgentDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already exists");

        var agent = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            Role = "Agent",
            CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")),
            UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
        };

        _context.Users.Add(agent);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Agent created successfully" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAgent(int id, [FromBody] UpdateAgentDto dto)
    {
        var agent = await _context.Users.FindAsync(id);
        if (agent == null || agent.Role != "Agent")
            return NotFound("Agent not found");

        if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
            return BadRequest("Email already exists");

        agent.Name = dto.Name;
        agent.Email = dto.Email;
        agent.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        await _context.SaveChangesAsync();
        return Ok(new { message = "Agent updated successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgent(int id)
    {
        var agent = await _context.Users.FindAsync(id);
        if (agent == null || agent.Role != "Agent")
            return NotFound("Agent not found");

        _context.Users.Remove(agent);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Agent deleted successfully" });
    }
}

public record CreateAgentDto(string Name, string Email, string Password);
public record UpdateAgentDto(string Name, string Email);