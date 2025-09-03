using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Parent
{
    public int Id { get; set; }
    
    [Required]
    public int PersonId { get; set; }
    
    [StringLength(100)]
    public string? Occupation { get; set; }
    
    [StringLength(15)]
    public string? PhoneNumber { get; set; }
    
    [StringLength(100)]
    public string? Email { get; set; }
    
    [StringLength(200)]
    public string? Address { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Person Person { get; set; } = null!;
    public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
}