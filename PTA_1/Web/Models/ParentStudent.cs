using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class ParentStudent
{
    public int Id { get; set; }
    
    [Required]
    public int ParentId { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    
    [StringLength(50)]
    public string RelationshipType { get; set; } = "Guardian";
    
    public bool IsPrimaryContact { get; set; } = false;
    public bool HasPickupAuthorization { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Parent Parent { get; set; } = null!;
    public Student Student { get; set; } = null!;
}