using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Person> Persons { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<ParentStudent> ParentStudents { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Person relationships
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Person)
            .WithOne(p => p.Student)
            .HasForeignKey<Student>(s => s.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Parent>()
            .HasOne(p => p.Person)
            .WithOne(pe => pe.Parent)
            .HasForeignKey<Parent>(p => p.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.Person)
            .WithOne(p => p.Teacher)
            .HasForeignKey<Teacher>(t => t.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // ParentStudent many-to-many relationship
        modelBuilder.Entity<ParentStudent>()
            .HasOne(ps => ps.Parent)
            .WithMany(p => p.ParentStudents)
            .HasForeignKey(ps => ps.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ParentStudent>()
            .HasOne(ps => ps.Student)
            .WithMany(s => s.ParentStudents)
            .HasForeignKey(ps => ps.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Indexes for better performance
        modelBuilder.Entity<Person>()
            .HasIndex(p => new { p.LastName, p.FirstName });
            
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.StudentId)
            .IsUnique();
            
        modelBuilder.Entity<Teacher>()
            .HasIndex(t => t.EmployeeId)
            .IsUnique();
    }
}
