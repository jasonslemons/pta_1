using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public enum ParticipationType
{
    Participant = 1,
    Volunteer = 2
}

public enum SignupStatus
{
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Waitlist = 4
}

public class ActivitySignup
{
    public int Id { get; set; }
    
    [Required]
    public int ActivityId { get; set; }
    
    [Required]
    public int PersonId { get; set; }
    
    [Required]
    public ParticipationType ParticipationType { get; set; }
    
    public SignupStatus Status { get; set; } = SignupStatus.Pending;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    [StringLength(100)]
    public string? EmergencyContact { get; set; }
    
    [StringLength(15)]
    public string? EmergencyPhone { get; set; }
    
    public DateTime SignupDate { get; set; } = DateTime.UtcNow;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Activity Activity { get; set; } = null!;
    public Person Person { get; set; } = null!;
    
    // Helper properties
    public string ParticipationTypeDisplay => ParticipationType == ParticipationType.Volunteer ? "Volunteer" : "Participant";
    public string StatusDisplay => Status.ToString();
}