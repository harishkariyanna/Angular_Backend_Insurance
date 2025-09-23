using System.ComponentModel.DataAnnotations;

namespace insu.Models;

public class Policy
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public decimal Premium { get; set; }
    
    public string? ImagePath { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}