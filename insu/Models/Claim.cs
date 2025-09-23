using System.ComponentModel.DataAnnotations;

namespace insu.Models;

public class Claim
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public decimal Amount { get; set; }
    
    public string Status { get; set; } = "Pending";
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int PolicyApplicationId { get; set; }
    public PolicyApplication PolicyApplication { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}