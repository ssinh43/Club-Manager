using clubmanager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace clubmanager.Controllers;
public class ClubRegistrationController(DataContext db) : Controller
{
    [AllowAnonymous][HttpGet]
    public async Task<IActionResult> Join(int clubId)
    {
        var club = await db.Clubs.AsNoTracking().SingleOrDefaultAsync(c => c.Id == clubId && c.Status == "Hoạt động");
        if (club == null) return NotFound();
        ViewBag.Club = club; return View(new JoinRequest { ClubId = clubId });
    }
    [AllowAnonymous][HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Join([Bind("ClubId,StudentCode,FullName,ClassName,Email,Message")] JoinRequest request)
    {
        var club = await db.Clubs.FindAsync(request.ClubId);
        if (club == null || club.Status != "Hoạt động") return NotFound();
        if (!string.IsNullOrWhiteSpace(request.StudentCode))
        {
            request.StudentCode = request.StudentCode.Trim().ToUpperInvariant();
            if (await db.ClubMembers.AnyAsync(m => m.ClubId == request.ClubId && m.StudentCode == request.StudentCode)) ModelState.AddModelError(nameof(request.StudentCode), "Bạn đã là thành viên của CLB.");
            if (await db.JoinRequests.AnyAsync(r => r.ClubId == request.ClubId && r.StudentCode == request.StudentCode && r.Status == "Chờ duyệt")) ModelState.AddModelError(nameof(request.StudentCode), "Đã có đơn đăng ký đang chờ duyệt.");
        }
        if (!ModelState.IsValid) { ViewBag.Club = club; return View(request); }
        db.JoinRequests.Add(request); await db.SaveChangesAsync();
        return View("Joined");
    }
}
