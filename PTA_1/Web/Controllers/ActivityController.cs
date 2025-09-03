using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class ActivityController : Controller
{
    private readonly ApplicationDbContext _context;

    public ActivityController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var activities = await _context.Activities
            .Where(a => a.IsActive)
            .OrderBy(a => a.StartDate)
            .ToListAsync();
        return View(activities);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var activity = await _context.Activities
            .Include(a => a.Tasks.Where(t => t.IsActive))
            .Include(a => a.Stations.Where(s => s.IsActive))
            .Include(a => a.ActivitySignups)
                .ThenInclude(signup => signup.Person)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (activity == null)
            return NotFound();

        return View(activity);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,Location,StartDate,EndDate,MaxParticipants,MaxVolunteers")] Models.Activity activity)
    {
        if (ModelState.IsValid)
        {
            activity.IsActive = true;
            activity.CreatedAt = DateTime.UtcNow;
            activity.UpdatedAt = DateTime.UtcNow;
            
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Details), new { id = activity.Id });
        }
        return View(activity);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var activity = await _context.Activities.FindAsync(id);
        if (activity == null)
            return NotFound();

        return View(activity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,StartDate,EndDate,MaxParticipants,MaxVolunteers,IsActive")] Models.Activity activity)
    {
        if (id != activity.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                activity.UpdatedAt = DateTime.UtcNow;
                _context.Update(activity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(activity.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(activity);
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return View("Index", await _context.Activities.Where(a => a.IsActive).OrderBy(a => a.StartDate).ToListAsync());

        var activities = await _context.Activities
            .Where(a => a.IsActive && 
                       (a.Title.Contains(searchTerm) ||
                        (a.Description != null && a.Description.Contains(searchTerm)) ||
                        (a.Location != null && a.Location.Contains(searchTerm))))
            .OrderBy(a => a.StartDate)
            .ToListAsync();

        ViewBag.SearchTerm = searchTerm;
        return View("Index", activities);
    }

    public async Task<IActionResult> Signup(int id, ParticipationType participationType)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null)
            return NotFound();

        ViewBag.Activity = activity;
        ViewBag.ParticipationType = participationType;
        
        var people = await _context.Persons
            .Include(p => p.Student)
            .Include(p => p.Parent)
            .Include(p => p.Teacher)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();

        return View(people);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessSignup(int activityId, int personId, ParticipationType participationType, string? notes, string? emergencyContact, string? emergencyPhone)
    {
        var activity = await _context.Activities.FindAsync(activityId);
        var person = await _context.Persons.FindAsync(personId);

        if (activity == null || person == null)
            return NotFound();

        // Check if person is already signed up for this activity
        var existingSignup = await _context.ActivitySignups
            .FirstOrDefaultAsync(s => s.ActivityId == activityId && s.PersonId == personId);

        if (existingSignup != null)
        {
            TempData["Error"] = "This person is already signed up for this activity.";
            return RedirectToAction(nameof(Details), new { id = activityId });
        }

        var signup = new ActivitySignup
        {
            ActivityId = activityId,
            PersonId = personId,
            ParticipationType = participationType,
            Status = SignupStatus.Confirmed,
            Notes = notes,
            EmergencyContact = emergencyContact,
            EmergencyPhone = emergencyPhone,
            SignupDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ActivitySignups.Add(signup);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"{person.FirstName} {person.LastName} has been signed up as a {participationType.ToString().ToLower()}.";
        return RedirectToAction(nameof(Details), new { id = activityId });
    }

    private bool ActivityExists(int id)
    {
        return _context.Activities.Any(e => e.Id == id);
    }
}