using clubmanager.Areas.Admin.Filters;
using clubmanager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace clubmanager.Areas.Admin.Controllers;
[Area("Admin")]
[ClubAdminSession]
public class RequestsController(DataContext db) : Controller
{
    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Review(int id, string decision)
    {
        var request = await db.JoinRequests.FindAsync(id);
        if (request == null) return NotFound();
        if (request.Status != "Chờ duyệt" || (decision != "Approve" && decision != "Reject")) return BadRequest();
        var club = await db.Clubs.FindAsync(request.ClubId);
        if (club == null) return NotFound();
        if (decision == "Approve")
        {
            if (club.Status != "Hoạt động" || await db.ClubMembers.CountAsync(m => m.ClubId == club.Id) >= club.Capacity || await db.ClubMembers.AnyAsync(m => m.ClubId == club.Id && m.StudentCode == request.StudentCode))
            {
                TempData["Error"] = "Không thể duyệt: CLB tạm dừng, đã đủ chỗ hoặc học sinh đã tham gia.";
                return RedirectToAction("Details", "Clubs", new { id = club.Id });
            }
            db.ClubMembers.Add(new ClubMember { ClubId = club.Id, StudentCode = request.StudentCode, FullName = request.FullName, ClassName = request.ClassName, Email = request.Email, Role = "Thành viên" });
            request.Status = "Đã duyệt";
        }
        else request.Status = "Từ chối";
        request.ReviewedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        TempData["Success"] = decision == "Approve" ? "Đã duyệt đăng ký và thêm thành viên." : "Đã từ chối đăng ký.";
        return RedirectToAction("Details", "Clubs", new { id = club.Id });
    }
}
