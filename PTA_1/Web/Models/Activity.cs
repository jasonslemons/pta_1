using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Activity
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(200)]
    public string? Location { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public int MaxParticipants { get; set; } = 0; // 0 means unlimited
    
    public int MaxVolunteers { get; set; } = 0; // 0 means unlimited
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<Station> Stations { get; set; } = new List<Station>();
    public ICollection<ActivitySignup> ActivitySignups { get; set; } = new List<ActivitySignup>();
    
    // Helper properties
    public bool HasDateRange => EndDate.HasValue && EndDate.Value.Date != StartDate.Date;
    public string DateDisplay => HasDateRange ? 
        $"{StartDate:MM/dd/yyyy} - {EndDate:MM/dd/yyyy}" : 
        StartDate.ToString("MM/dd/yyyy");
}