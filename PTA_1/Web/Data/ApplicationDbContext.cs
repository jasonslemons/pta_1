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
    
    // Activity-related entities
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Models.Task> Tasks { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<ActivitySignup> ActivitySignups { get; set; }
    public DbSet<TaskSignup> TaskSignups { get; set; }
    public DbSet<StationSignup> StationSignups { get; set; }
    
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
            
        // Activity relationships
        modelBuilder.Entity<Models.Task>()
            .HasOne(t => t.Activity)
            .WithMany(a => a.Tasks)
            .HasForeignKey(t => t.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Station>()
            .HasOne(s => s.Activity)
            .WithMany(a => a.Stations)
            .HasForeignKey(s => s.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ActivitySignup>()
            .HasOne(signup => signup.Activity)
            .WithMany(a => a.ActivitySignups)
            .HasForeignKey(signup => signup.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ActivitySignup>()
            .HasOne(signup => signup.Person)
            .WithMany()
            .HasForeignKey(signup => signup.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<TaskSignup>()
            .HasOne(ts => ts.Task)
            .WithMany(t => t.TaskSignups)
            .HasForeignKey(ts => ts.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<TaskSignup>()
            .HasOne(ts => ts.Person)
            .WithMany()
            .HasForeignKey(ts => ts.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<StationSignup>()
            .HasOne(ss => ss.Station)
            .WithMany(s => s.StationSignups)
            .HasForeignKey(ss => ss.StationId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<StationSignup>()
            .HasOne(ss => ss.Person)
            .WithMany()
            .HasForeignKey(ss => ss.PersonId)
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
            
        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.StartDate);
            
        modelBuilder.Entity<ActivitySignup>()
            .HasIndex(signup => new { signup.ActivityId, signup.PersonId })
            .IsUnique();
            
        modelBuilder.Entity<TaskSignup>()
            .HasIndex(ts => new { ts.TaskId, ts.PersonId })
            .IsUnique();
            
        modelBuilder.Entity<StationSignup>()
            .HasIndex(ss => new { ss.StationId, ss.PersonId })
            .IsUnique();
    }
}
