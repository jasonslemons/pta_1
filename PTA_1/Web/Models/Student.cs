using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Student
{
    public int Id { get; set; }
    
    [Required]
    public int PersonId { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Grade { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? StudentId { get; set; }
    
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Person Person { get; set; } = null!;
    public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
}