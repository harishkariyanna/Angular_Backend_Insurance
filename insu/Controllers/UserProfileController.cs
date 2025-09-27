using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using insu.Data;
using insu.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace insu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public UserProfileController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<User>> GetProfile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        user.Password = ""; // Don't return password
        return user;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<ActionResult<User>> GetUserProfile(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Password = ""; // Don't return password
        return user;
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        user.Name = request.Name;
        user.Phone = request.Phone;
        user.Address = request.Address;
        user.DateOfBirth = request.DateOfBirth;
        user.Gender = request.Gender;


        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("upload-picture")]
    [Authorize]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads", "profiles");
        Directory.CreateDirectory(uploadsDir);

        // Delete old profile picture if exists
        var existingFiles = Directory.GetFiles(uploadsDir, $"profile_{userId}_*");
        foreach (var existingFile in existingFiles)
        {
            System.IO.File.Delete(existingFile);
        }

        var fileName = $"profile_{userId}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Profile picture uploaded successfully" });
    }

    [HttpGet("picture/{userId}")]
    public async Task<IActionResult> GetProfilePicture(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads", "profiles");
        var possibleExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        
        foreach (var ext in possibleExtensions)
        {
            var filePath = Path.Combine(uploadsDir, $"profile_{userId}{ext}");
            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var contentType = ext.ToLower() switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "image/jpeg"
                };
                return File(fileBytes, contentType);
            }
        }
        
        return NotFound("Profile picture not found");
    }

    [HttpGet("download/{userId}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> DownloadCustomerProfile(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var policies = await _context.PolicyApplications
            .Include(p => p.Policy)
            .Where(p => p.UserId == userId && p.Status == "Approved")
            .ToListAsync();

        var profileData = new
        {
            CustomerInfo = new
            {
                user.Name,
                user.Email,
                user.Phone,
                user.Address,
                user.DateOfBirth,
                user.Gender
            },
            Policies = policies.Select(p => new
            {
                PolicyName = p.Policy.Name,
                p.DurationMonths,
                Premium = p.Policy.Premium * p.DurationMonths / 12,
                AppliedDate = p.CreatedAt,
                p.Status
            })
        };

        try
        {
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Inch);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Customer Profile - {user.Name}")
                            .FontSize(16).Bold();

                        column.Item().PaddingTop(20);

                        column.Item().Text("Customer Information")
                            .FontSize(12).Bold();

                        column.Item().PaddingTop(10);
                        column.Item().Text($"Name: {user.Name}");
                        column.Item().Text($"Email: {user.Email}");
                        column.Item().Text($"Phone: {user.Phone ?? "N/A"}");
                        column.Item().Text($"Address: {user.Address ?? "N/A"}");
                        column.Item().Text($"Date of Birth: {user.DateOfBirth?.ToString("dd/MM/yyyy") ?? "N/A"}");
                        column.Item().Text($"Gender: {user.Gender ?? "N/A"}");


                        column.Item().PaddingTop(20);
                        column.Item().Text("Policies").FontSize(12).Bold();
                        column.Item().PaddingTop(10);

                        if (policies.Any())
                        {
                            foreach (var policy in policies)
                            {
                                column.Item().Text($"Policy: {policy.Policy.Name}");
                                column.Item().Text($"Duration: {policy.DurationMonths} months");
                                column.Item().Text($"Premium: ₹{policy.Policy.Premium * policy.DurationMonths / 12:F2}");
                                column.Item().Text($"Applied: {policy.CreatedAt:dd/MM/yyyy}");
                                column.Item().Text($"Status: {policy.Status}");
                                column.Item().PaddingTop(10);
                            }
                        }
                        else
                        {
                            column.Item().Text("No policies found.");
                        }
                    });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"{user.Name.Replace(" ", "_")}_profile.pdf");
        }
        catch (Exception ex)
        {
            // Fallback to simple text format if PDF generation fails
            var textContent = $"Customer Profile - {user.Name}\n\n" +
                             $"Name: {user.Name}\n" +
                             $"Email: {user.Email}\n" +
                             $"Phone: {user.Phone ?? "N/A"}\n" +
                             $"Address: {user.Address ?? "N/A"}\n" +
                             $"Date of Birth: {user.DateOfBirth?.ToString("dd/MM/yyyy") ?? "N/A"}\n" +
                             $"Gender: {user.Gender ?? "N/A"}\n" +
                             "\n" +
                             "Policies:\n" +
                             string.Join("\n", policies.Select(p =>
                                 $"- {p.Policy.Name} ({p.DurationMonths} months, ₹{p.Policy.Premium * p.DurationMonths / 12:F2})"));

            var bytes = System.Text.Encoding.UTF8.GetBytes(textContent);
            return File(bytes, "text/plain", $"{user.Name.Replace(" ", "_")}_profile.txt");
        }
    }
}

public class UpdateProfileRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }

}