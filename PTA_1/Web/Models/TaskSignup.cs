using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class TaskSignup
{
    public int Id { get; set; }
    
    [Required]
    public int TaskId { get; set; }
    
    [Required]
    public int PersonId { get; set; }
    
    public SignupStatus Status { get; set; } = SignupStatus.Pending;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    public DateTime SignupDate { get; set; } = DateTime.UtcNow;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Task Task { get; set; } = null!;
    public Person Person { get; set; } = null!;
    
    // Helper properties
    public string StatusDisplay => Status.ToString();
}