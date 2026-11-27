using clubmanager.Models;
using clubmanager.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clubmanager.Controllers;

public class ClubController(DataContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var clubs = await db.Clubs
            .AsNoTracking()
            .Where(club => club.Status == "Hoạt động")
            .OrderBy(club => club.Name)
            .ToListAsync();

        return View(clubs);
    }

    public async Task<IActionResult> Joined()
    {
        if (!Functions.IsLogin())
            return RedirectToAction("Index", "Login");

        var email = Functions._Email.Trim().ToLowerInvariant();
        var memberships = await db.ClubMembers
            .AsNoTracking()
            .Where(member => member.Email != null && member.Email.Trim().ToLower() == email)
            .Include(member => member.Club)
            .OrderByDescending(member => member.JoinedOn)
            .ToListAsync();

        var joinedClubs = memberships
            .Where(member => member.Club != null)
            .GroupBy(member => member.ClubId)
            .Select(group => group.First())
            .ToList();

        return View(joinedClubs);
    }
}
