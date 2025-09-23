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
public class UploadsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public UploadsController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Upload>>> GetUploads()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var isAdmin = User.IsInRole("Admin");

        var query = _context.Uploads.Include(u => u.User);
        return isAdmin ? await query.ToListAsync() : await query.Where(u => u.UserId == userId).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Upload>> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var upload = new Upload
        {
            FileName = file.FileName,
            FilePath = $"uploads/{fileName}",
            FileType = file.ContentType,
            FileSize = file.Length,
            UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
        };

        _context.Uploads.Add(upload);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUpload), new { id = upload.Id }, upload);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Upload>> GetUpload(int id)
    {
        var upload = await _context.Uploads.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == id);
        if (upload == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Admin") && upload.UserId != userId)
            return Forbid();

        return upload;
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var upload = await _context.Uploads.FindAsync(id);
        if (upload == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Admin") && upload.UserId != userId)
            return Forbid();

        var filePath = Path.Combine(_env.ContentRootPath, upload.FilePath);
        if (!System.IO.File.Exists(filePath))
            return NotFound("File not found on disk");

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, upload.FileType, upload.FileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUpload(int id)
    {
        var upload = await _context.Uploads.FindAsync(id);
        if (upload == null) return NotFound();

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Admin") && upload.UserId != userId)
            return Forbid();

        var filePath = Path.Combine(_env.ContentRootPath, upload.FilePath);
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        _context.Uploads.Remove(upload);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}