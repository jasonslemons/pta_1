using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class StationSignup
{
    public int Id { get; set; }
    
    [Required]
    public int StationId { get; set; }
    
    [Required]
    public int PersonId { get; set; }
    
    public SignupStatus Status { get; set; } = SignupStatus.Pending;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    [StringLength(100)]
    public string? ParentGuardian { get; set; }
    
    public DateTime SignupDate { get; set; } = DateTime.UtcNow;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Station Station { get; set; } = null!;
    public Person Person { get; set; } = null!;
    
    // Helper properties
    public string StatusDisplay => Status.ToString();
}