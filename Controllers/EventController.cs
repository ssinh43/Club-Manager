using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using clubmanager.Models;
using clubmanager.Utilities;

namespace clubmanager.Controllers
{
    public class EventController : Controller
    {
        private readonly DataContext _context;

        public EventController(DataContext context)
        {
            _context = context;
        }

        // Lấy user đang đăng nhập theo đúng cách LoginController lưu (0 = chưa đăng nhập)
        private int? GetCurrentUserId() => Functions._UserID > 0 ? Functions._UserID : (int?)null;

        // Tất cả sự kiện (mới nhất lên đầu)
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .OrderByDescending(e => e.EventDate)
                .ThenByDescending(e => e.Id)
                .ToListAsync();

            var userId = GetCurrentUserId();
            ViewBag.RegisteredIds = userId == null
                ? new HashSet<int>()
                : (await _context.EventRegistrations
                    .Where(r => r.UserId == userId)
                    .Select(r => r.EventId)
                    .ToListAsync()).ToHashSet();

            return View(events);
        }

        // Sự kiện đã tham gia của user đang đăng nhập
        public async Task<IActionResult> Registered()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("Index", "Login");

            var registrations = await _context.EventRegistrations
                .Where(r => r.UserId == userId.Value)
                .Join(_context.Events,
                      r => r.EventId,
                      e => e.Id,
                      (r, e) => new { Event = e, r.RegisteredAt })
                .OrderByDescending(x => x.RegisteredAt)
                .ToListAsync();

            ViewBag.RegisteredAt = registrations.ToDictionary(x => x.Event.Id, x => x.RegisteredAt);

            return View(registrations.Select(x => x.Event).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("Index", "Login");

            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            if (ev.EventDate.HasValue && ev.EventDate.Value < DateTime.Now)
            {
                TempData["Error"] = "Sự kiện này đã kết thúc.";
                return RedirectToAction(nameof(Index));
            }

            var exists = await _context.EventRegistrations
                .AnyAsync(r => r.EventId == id && r.UserId == userId);

            if (!exists)
            {
                _context.EventRegistrations.Add(new EventRegistration
                {
                    EventId = id,
                    UserId = userId.Value
                });
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Bạn đã đăng ký tham gia \"{ev.EventName}\".";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}