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
    public async Task<ActionResult<IEnumerable<object>>> GetApplications()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = _context.PolicyApplications.Include(a => a.User).Include(a => a.Policy).Include(a => a.AssignedAgent);

        var applications = User.IsInRole("Admin") ? await query.ToListAsync() :
                          User.IsInRole("Agent") ? await query.Where(a => a.AgentId == userId).ToListAsync() :
                          await query.Where(a => a.UserId == userId).ToListAsync();

        return User.IsInRole("Agent") ?
            applications.Select(a => new { a.Id, a.UserId, a.PolicyId, a.DurationMonths, a.Status, a.AdminComments, a.CreatedAt, a.UpdatedAt, a.User, a.Policy, a.AssignedAgent, HasDocs = !string.IsNullOrEmpty(a.AadharCardPath) }).ToList<object>() :
            applications.Select(a => new { a.Id, a.UserId, a.PolicyId, a.DurationMonths, a.Status, a.AdminComments, a.CreatedAt, a.UpdatedAt, a.User, a.Policy, a.AssignedAgent }).ToList<object>();
    }

    [HttpPost]
    public async Task<ActionResult<PolicyApplication>> CreateApplication([FromForm] PolicyApplicationDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var existingAssignment = await _context.PolicyApplications.Where(pa => pa.UserId == userId && pa.AgentId != null).FirstOrDefaultAsync();

        var application = new PolicyApplication
        {
            UserId = userId,
            PolicyId = dto.PolicyId,
            DurationMonths = dto.DurationMonths,
            AgentId = existingAssignment?.AgentId,
            Status = existingAssignment?.AgentId != null ? "Assigned" : "Pending",
            AadharCardPath = await SaveFileAsync(dto.AadharCard, "aadhar"),
            PanCardPath = await SaveFileAsync(dto.PanCard, "pan"),
            BankPassbookPath = await SaveFileAsync(dto.BankPassbook, "passbook")
        };

        _context.PolicyApplications.Add(application);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PolicyApplication>> GetApplication(int id)
    {
        var application = await _context.PolicyApplications.Include(a => a.User).Include(a => a.Policy).Include(a => a.AssignedAgent).FirstOrDefaultAsync(a => a.Id == id);
        if (application == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if ((User.IsInRole("Agent") && application.AgentId != userId) || (!User.IsInRole("Agent") && !User.IsInRole("Admin") && application.UserId != userId))
            return Forbid();

        return application;
    }

    [HttpGet("{id}/documents/{docType}")]
    public async Task<IActionResult> GetDocument(int id, string docType)
    {
        var application = await _context.PolicyApplications.FindAsync(id);
        if (application == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if ((User.IsInRole("Agent") && application.AgentId != userId) || (!User.IsInRole("Agent") && application.UserId != userId))
            return Forbid();

        var filePath = docType.ToLower() switch { "aadhar" => application.AadharCardPath, "pan" => application.PanCardPath, "passbook" => application.BankPassbookPath, _ => null };
        if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath)) return NotFound();

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        var contentType = Path.GetExtension(filePath).ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };

        return File(fileBytes, contentType, Path.GetFileName(filePath));
    }

    [HttpPut("{id}/assign-agent")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignAgent(int id, AssignAgentDto dto)
    {
        var application = await _context.PolicyApplications.FindAsync(id);
        if (application == null) return NotFound();
        application.AgentId = dto.AgentId; application.Status = "Assigned"; application.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> ApproveApplication(int id, ApprovalDto dto)
    {
        var application = await _context.PolicyApplications.FindAsync(id);
        if (application == null) return NotFound();
        application.Status = dto.Status; application.AdminComments = dto.Comments; application.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("agents")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAgents() => await _context.Users.Where(u => u.Role == "Agent").ToListAsync();

    [HttpGet("agent-customers")]
    [Authorize(Roles = "Agent")]
    public async Task<ActionResult<IEnumerable<object>>> GetAgentCustomers()
    {
        var agentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var applications = await _context.PolicyApplications.Include(a => a.User).Include(a => a.Policy).Where(a => a.AgentId == agentId).ToListAsync();
        return applications.Select(a => new { a.Id, a.UserId, a.PolicyId, a.DurationMonths, a.Status, a.AdminComments, a.CreatedAt, a.UpdatedAt, a.User, a.Policy, HasDocs = !string.IsNullOrEmpty(a.AadharCardPath) }).ToList<object>();
    }

    private async Task<string?> SaveFileAsync(IFormFile? file, string prefix)
    {
        if (file == null || file.Length == 0) return null;
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(uploadsFolder);
        var filePath = Path.Combine(uploadsFolder, $"{prefix}_{Guid.NewGuid()}_{file.FileName}");
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return filePath;
    }
}

public record PolicyApplicationDto(int PolicyId, int DurationMonths, IFormFile AadharCard, IFormFile PanCard, IFormFile BankPassbook);
public record ApprovalDto(string Status, string? Comments);
public record AssignAgentDto(int AgentId);