using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class ParentController : Controller
{
    private readonly ApplicationDbContext _context;

    public ParentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var parents = await _context.Parents
            .Include(p => p.Person)
            .ToListAsync();
        return View(parents);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var parent = await _context.Parents
            .Include(p => p.Person)
            .Include(p => p.ParentStudents)
                .ThenInclude(ps => ps.Student)
                    .ThenInclude(s => s.Person)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (parent == null)
            return NotFound();

        return View(parent);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,MiddleName,Suffix,Birthday,SSN,Occupation,PhoneNumber,Email,Address")] ParentCreateViewModel model)
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

            var parent = new Parent
            {
                PersonId = person.Id,
                Occupation = model.Occupation,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return View("Index", await _context.Parents.Include(p => p.Person).ToListAsync());

        var parents = await _context.Parents
            .Include(p => p.Person)
            .Where(p => p.Person.FirstName.Contains(searchTerm) ||
                       p.Person.LastName.Contains(searchTerm) ||
                       (p.Email != null && p.Email.Contains(searchTerm)) ||
                       (p.PhoneNumber != null && p.PhoneNumber.Contains(searchTerm)))
            .ToListAsync();

        ViewBag.SearchTerm = searchTerm;
        return View("Index", parents);
    }
}

public class ParentCreateViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Suffix { get; set; }
    public DateTime? Birthday { get; set; }
    public string? SSN { get; set; }
    public string? Occupation { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}