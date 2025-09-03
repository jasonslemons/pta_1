using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Station
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
    
    public int MaxParticipants { get; set; } = 0; // 0 means unlimited
    
    public int MinAge { get; set; } = 0;
    public int MaxAge { get; set; } = 0; // 0 means no age limit
    
    [StringLength(200)]
    public string? Requirements { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int SortOrder { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Activity Activity { get; set; } = null!;
    public ICollection<StationSignup> StationSignups { get; set; } = new List<StationSignup>();
    
    // Helper properties
    public string TimeDisplay => StartTime.HasValue && EndTime.HasValue ?
        $"{StartTime:h:mm tt} - {EndTime:h:mm tt}" :
        StartTime?.ToString("h:mm tt") ?? "Time TBD";
        
    public string AgeRangeDisplay
    {
        get
        {
            if (MinAge == 0 && MaxAge == 0) return "All Ages";
            if (MinAge > 0 && MaxAge == 0) return $"{MinAge}+ years";
            if (MinAge == 0 && MaxAge > 0) return $"Up to {MaxAge} years";
            return $"{MinAge}-{MaxAge} years";
        }
    }
    
    public int CurrentParticipantCount => StationSignups.Count;
    public bool IsFull => MaxParticipants > 0 && CurrentParticipantCount >= MaxParticipants;
    public int SpotsRemaining => MaxParticipants > 0 ? Math.Max(0, MaxParticipants - CurrentParticipantCount) : int.MaxValue;
}