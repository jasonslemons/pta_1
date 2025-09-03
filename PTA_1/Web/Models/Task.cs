using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Task
{
    public int Id { get; set; }
    
    [Required]
    public int ActivityId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(200)]
    public string? Location { get; set; }
    
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    
    public int MaxVolunteers { get; set; } = 1;
    
    public bool IsRequired { get; set; } = false;
    
    public bool IsActive { get; set; } = true;
    
    public int SortOrder { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Activity Activity { get; set; } = null!;
    public ICollection<TaskSignup> TaskSignups { get; set; } = new List<TaskSignup>();
    
    // Helper properties
    public string TimeDisplay => StartTime.HasValue && EndTime.HasValue ?
        $"{StartTime:h:mm tt} - {EndTime:h:mm tt}" :
        StartTime?.ToString("h:mm tt") ?? "Time TBD";
        
    public int CurrentVolunteerCount => TaskSignups.Count;
    public bool IsFull => MaxVolunteers > 0 && CurrentVolunteerCount >= MaxVolunteers;
    public int SpotsRemaining => MaxVolunteers > 0 ? Math.Max(0, MaxVolunteers - CurrentVolunteerCount) : int.MaxValue;
}