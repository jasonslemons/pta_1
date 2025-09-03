using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class TeacherController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeacherController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var teachers = await _context.Teachers
            .Include(t => t.Person)
            .ToListAsync();
        return View(teachers);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var teacher = await _context.Teachers
            .Include(t => t.Person)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher == null)
            return NotFound();

        return View(teacher);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,MiddleName,Suffix,Birthday,SSN,Grade,Classroom,Subject,EmployeeId,PhoneNumber,Email")] TeacherCreateViewModel model)
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

            var teacher = new Teacher
            {
                PersonId = person.Id,
                Grade = model.Grade,
                Classroom = model.Classroom,
                Subject = model.Subject,
                EmployeeId = model.EmployeeId,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                HireDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return View("Index", await _context.Teachers.Include(t => t.Person).ToListAsync());

        var teachers = await _context.Teachers
            .Include(t => t.Person)
            .Where(t => t.Person.FirstName.Contains(searchTerm) ||
                       t.Person.LastName.Contains(searchTerm) ||
                       (t.Grade != null && t.Grade.Contains(searchTerm)) ||
                       (t.Classroom != null && t.Classroom.Contains(searchTerm)) ||
                       (t.Subject != null && t.Subject.Contains(searchTerm)) ||
                       (t.EmployeeId != null && t.EmployeeId.Contains(searchTerm)))
            .ToListAsync();

        ViewBag.SearchTerm = searchTerm;
        return View("Index", teachers);
    }
}

public class TeacherCreateViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Suffix { get; set; }
    public DateTime? Birthday { get; set; }
    public string? SSN { get; set; }
    public string? Grade { get; set; }
    public string? Classroom { get; set; }
    public string? Subject { get; set; }
    public string? EmployeeId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}