using clubmanager.Areas.Admin.Filters;
using clubmanager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace clubmanager.Areas.Admin.Controllers;
[Area("Admin")]
[ClubAdminSession]
public class MembersController(DataContext db) : Controller
{
    public async Task<IActionResult> Create(int clubId)
    {
        if (!await db.Clubs.AnyAsync(c => c.Id == clubId)) return NotFound();
        ViewBag.Club = await db.Clubs.FindAsync(clubId);
        return View("Form", new ClubMember { ClubId = clubId });
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ClubId,StudentCode,FullName,ClassName,Email,Phone,Role,JoinedOn")] ClubMember member)
    {
        var club = await db.Clubs.FindAsync(member.ClubId);
        if (club == null) return NotFound();
        await ValidateMember(member, club);
        if (!ModelState.IsValid) { ViewBag.Club = club; return View("Form", member); }
        member.StudentCode = member.StudentCode.Trim().ToUpperInvariant();
        db.ClubMembers.Add(member); await db.SaveChangesAsync();
        TempData["Success"] = "Đã thêm thành viên.";
        return RedirectToAction("Details", "Clubs", new { id = member.ClubId });
    }
    public async Task<IActionResult> Edit(int id)
    {
        var member = await db.ClubMembers.FindAsync(id);
        if (member == null) return NotFound();
        ViewBag.Club = await db.Clubs.FindAsync(member.ClubId);
        return View("Form", member);
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ClubId,StudentCode,FullName,ClassName,Email,Phone,Role,JoinedOn")] ClubMember input)
    {
        if (id != input.Id) return BadRequest();
        var original = await db.ClubMembers.FindAsync(id);
        if (original == null || original.ClubId != input.ClubId) return BadRequest();
        var club = await db.Clubs.FindAsync(input.ClubId);
        if (club == null) return NotFound();
        await ValidateMember(input, club, id);
        if (!ModelState.IsValid) { ViewBag.Club = club; return View("Form", input); }
        original.StudentCode = input.StudentCode.Trim().ToUpperInvariant(); original.FullName = input.FullName;
        original.ClassName = input.ClassName; original.Email = input.Email; original.Phone = input.Phone;
        original.Role = input.Role; original.JoinedOn = input.JoinedOn;
        await db.SaveChangesAsync(); TempData["Success"] = "Đã cập nhật thành viên.";
        return RedirectToAction("Details", "Clubs", new { id = input.ClubId });
    }
    private async Task ValidateMember(ClubMember member, Club club, int excludeId = 0)
    {
        if (!new[] { "Chủ nhiệm", "Phó chủ nhiệm", "Thành viên" }.Contains(member.Role)) ModelState.AddModelError(nameof(member.Role), "Vai trò không hợp lệ.");
        if (!string.IsNullOrWhiteSpace(member.StudentCode) && await db.ClubMembers.AnyAsync(m => m.ClubId == club.Id && m.StudentCode == member.StudentCode.Trim().ToUpper() && m.Id != excludeId)) ModelState.AddModelError(nameof(member.StudentCode), "Học sinh đã là thành viên CLB.");
        if (excludeId == 0 && await db.ClubMembers.CountAsync(m => m.ClubId == club.Id) >= club.Capacity) ModelState.AddModelError("", "CLB đã đủ số thành viên.");
        if (member.Role == "Chủ nhiệm" && await db.ClubMembers.AnyAsync(m => m.ClubId == club.Id && m.Role == "Chủ nhiệm" && m.Id != excludeId)) ModelState.AddModelError(nameof(member.Role), "CLB đã có chủ nhiệm.");
    }
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var m = await db.ClubMembers.FindAsync(id);
        if (m == null) return NotFound();
        int clubId = m.ClubId;
        db.ClubMembers.Remove(m); await db.SaveChangesAsync();
        TempData["Success"] = "Đã xóa thành viên.";
        return RedirectToAction("Details", "Clubs", new { id = clubId });
    }
}
