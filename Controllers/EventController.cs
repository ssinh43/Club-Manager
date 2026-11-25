using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using clubmanager.Models;

namespace clubmanager.Controllers
{
    public class EventController : Controller
    {
        private readonly DataContext _context;

        public EventController(DataContext context)
        {
            _context = context;
        }

        // Danh sách sự kiện
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Where(x => x.IsActive)
                .OrderBy(x => x.StartDate)
                .ToListAsync();

            return View(events);
        }

        // Chi tiết sự kiện
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _context.Events
                .FirstOrDefaultAsync(x => x.EventID == id && x.IsActive);

            if (ev == null)
            {
                return NotFound();
            }

            return View(ev);
        }
    }
}