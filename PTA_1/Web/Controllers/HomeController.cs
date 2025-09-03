using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var activities = await _context.Activities
            .Include(a => a.ActivitySignups)
            .Where(a => a.IsActive && a.StartDate >= DateTime.Today)
            .OrderBy(a => a.StartDate)
            .Take(6)
            .ToListAsync();
            
        return View(activities);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> QuickSignup(int activityId, ParticipationType participationType)
    {
        var activity = await _context.Activities.FindAsync(activityId);
        if (activity == null)
        {
            TempData["Error"] = "Activity not found.";
            return RedirectToAction(nameof(Index));
        }

        // Get list of people for selection
        var people = await _context.Persons
            .Include(p => p.Student)
            .Include(p => p.Parent)
            .Include(p => p.Teacher)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();

        ViewBag.Activity = activity;
        ViewBag.ParticipationType = participationType;
        return View("SelectPerson", people);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessQuickSignup(int activityId, int personId, ParticipationType participationType, string? notes, string? emergencyContact, string? emergencyPhone)
    {
        var activity = await _context.Activities.FindAsync(activityId);
        var person = await _context.Persons.FindAsync(personId);

        if (activity == null || person == null)
        {
            TempData["Error"] = "Activity or person not found.";
            return RedirectToAction(nameof(Index));
        }

        // Check if person is already signed up for this activity
        var existingSignup = await _context.ActivitySignups
            .FirstOrDefaultAsync(s => s.ActivityId == activityId && s.PersonId == personId);

        if (existingSignup != null)
        {
            TempData["Error"] = $"{person.FirstName} {person.LastName} is already signed up for {activity.Title}.";
            return RedirectToAction(nameof(Index));
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

        TempData["Success"] = $"{person.FirstName} {person.LastName} has been successfully signed up for {activity.Title} as a {participationType.ToString().ToLower()}!";
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
