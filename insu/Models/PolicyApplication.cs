using System.ComponentModel.DataAnnotations;

namespace insu.Models;

public class PolicyApplication
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int PolicyId { get; set; }
    public Policy Policy { get; set; } = null!;
    
    [Required]
    public int DurationMonths { get; set; }
    
    public string Status { get; set; } = "Pending";
    
    public int? AgentId { get; set; }
    public User? AssignedAgent { get; set; }
    
    public string? AdminComments { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}