using clubmanager.Areas.Admin.Filters;
using clubmanager.Models;
using clubmanager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace clubmanager.Areas.Admin.Controllers;
[Area("Admin")]
[ClubAdminSession]
public class ClubsController(DataContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, string? sport, string? status)
    {
        var query = db.Clubs.Include(c => c.Members).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search) || x.Code.Contains(search));
        if (!string.IsNullOrWhiteSpace(sport)) query = query.Where(x => x.Sport == sport);
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
        var vm = new ClubListVm
        {
            Clubs = await query.OrderBy(x => x.Name).ToListAsync(), Search = search, Sport = sport, Status = status,
            Sports = await db.Clubs.Select(x => x.Sport).Distinct().OrderBy(x => x).ToListAsync(),
            Total = await db.Clubs.CountAsync(), Active = await db.Clubs.CountAsync(x => x.Status == "Hoạt động"),
            Members = await db.ClubMembers.CountAsync(), Pending = await db.JoinRequests.CountAsync(x => x.Status == "Chờ duyệt")
        };
        return View(vm);
    }
    public async Task<IActionResult> Details(int id)
    {
        var club = await db.Clubs.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
        if (club == null) return NotFound();
        return View(new ClubDetailsVm { Club = club,
            Members = await db.ClubMembers.AsNoTracking().Where(m => m.ClubId == id).OrderBy(m => m.Role == "Chủ nhiệm" ? 0 : m.Role == "Phó chủ nhiệm" ? 1 : 2).ThenBy(m => m.FullName).ToListAsync(),
            Requests = await db.JoinRequests.AsNoTracking().Where(r => r.ClubId == id).OrderByDescending(r => r.CreatedAt).ToListAsync() });
    }
    public IActionResult Create() => View("Form", new Club());
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Code,Name,Sport,Description,Location,Schedule,Capacity,Status,FoundedOn")] Club club)
    {
        await ValidateClub(club);
        if (!ModelState.IsValid) return View("Form", club);
        club.Code = club.Code.Trim().ToUpperInvariant();
        db.Clubs.Add(club); await db.SaveChangesAsync();
        TempData["Success"] = "Đã tạo câu lạc bộ thành công.";
        return RedirectToAction(nameof(Details), new { id = club.Id });
    }
    public async Task<IActionResult> Edit(int id)
    {
        var club = await db.Clubs.FindAsync(id);
        return club == null ? NotFound() : View("Form", club);
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Sport,Description,Location,Schedule,Capacity,Status,FoundedOn")] Club input)
    {
        if (id != input.Id) return BadRequest();
        var current = await db.Clubs.FindAsync(id);
        if (current == null) return NotFound();
        await ValidateClub(input, id);
        var count = await db.ClubMembers.CountAsync(x => x.ClubId == id);
        if (input.Capacity < count) ModelState.AddModelError(nameof(input.Capacity), "Sức chứa nhỏ hơn số thành viên hiện tại.");
        if (!ModelState.IsValid) return View("Form", input);
        current.Code = input.Code.Trim().ToUpperInvariant(); current.Name = input.Name; current.Sport = input.Sport;
        current.Description = input.Description; current.Location = input.Location; current.Schedule = input.Schedule;
        current.Capacity = input.Capacity; current.Status = input.Status; current.FoundedOn = input.FoundedOn;
        await db.SaveChangesAsync(); TempData["Success"] = "Đã cập nhật câu lạc bộ.";
        return RedirectToAction(nameof(Details), new { id });
    }
    private async Task ValidateClub(Club club, int excludeId = 0)
    {
        if (club.Status != "Hoạt động" && club.Status != "Tạm dừng") ModelState.AddModelError(nameof(club.Status), "Trạng thái không hợp lệ.");
        if (!string.IsNullOrWhiteSpace(club.Code) && await db.Clubs.AnyAsync(c => c.Code == club.Code.Trim().ToUpper() && c.Id != excludeId)) ModelState.AddModelError(nameof(club.Code), "Mã câu lạc bộ đã tồn tại.");
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var club = await db.Clubs.FindAsync(id);
        if (club == null) return NotFound();
        // Prevent accidental removal of student records.
        if (await db.ClubMembers.AnyAsync(m => m.ClubId == id) || await db.JoinRequests.AnyAsync(r => r.ClubId == id))
        {
            TempData["Error"] = "CLB đã có thành viên/đơn đăng ký. Hãy chuyển sang Tạm dừng thay vì xóa.";
            return RedirectToAction(nameof(Details), new { id });
        }
        db.Clubs.Remove(club); await db.SaveChangesAsync(); TempData["Success"] = "Đã xóa câu lạc bộ.";
        return RedirectToAction(nameof(Index));
    }
}
