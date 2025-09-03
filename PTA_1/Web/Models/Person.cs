using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class Person
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? MiddleName { get; set; }
    
    [StringLength(10)]
    public string? Suffix { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }
    
    [StringLength(11)]
    public string? SSN { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Student? Student { get; set; }
    public Parent? Parent { get; set; }
    public Teacher? Teacher { get; set; }
}