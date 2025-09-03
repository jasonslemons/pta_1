using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Teacher
{
    public int Id { get; set; }
    
    [Required]
    public int PersonId { get; set; }
    
    [StringLength(20)]
    public string? Grade { get; set; }
    
    [StringLength(50)]
    public string? Classroom { get; set; }
    
    [StringLength(100)]
    public string? Subject { get; set; }
    
    [StringLength(50)]
    public string? EmployeeId { get; set; }
    
    [StringLength(15)]
    public string? PhoneNumber { get; set; }
    
    [StringLength(100)]
    public string? Email { get; set; }
    
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Person Person { get; set; } = null!;
}