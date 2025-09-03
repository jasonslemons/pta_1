using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _context.Students
            .Include(s => s.Person)
            .ToListAsync();
        return View(students);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var student = await _context.Students
            .Include(s => s.Person)
            .Include(s => s.ParentStudents)
                .ThenInclude(ps => ps.Parent)
                    .ThenInclude(p => p.Person)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound();

        return View(student);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,MiddleName,Suffix,Birthday,SSN,Grade,StudentId")] StudentCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var person = new Person
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Suffix = model.Suffix,
                Birthday = model.Birthday,
                SSN = model.SSN,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            var student = new Student
            {
                PersonId = person.Id,
                Grade = model.Grade,
                StudentId = model.StudentId,
                EnrollmentDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return View("Index", await _context.Students.Include(s => s.Person).ToListAsync());

        var students = await _context.Students
            .Include(s => s.Person)
            .Where(s => s.Person.FirstName.Contains(searchTerm) ||
                       s.Person.LastName.Contains(searchTerm) ||
                       s.Grade.Contains(searchTerm) ||
                       (s.StudentId != null && s.StudentId.Contains(searchTerm)))
            .ToListAsync();

        ViewBag.SearchTerm = searchTerm;
        return View("Index", students);
    }
}

public class StudentCreateViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Suffix { get; set; }
    public DateTime? Birthday { get; set; }
    public string? SSN { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string? StudentId { get; set; }
}